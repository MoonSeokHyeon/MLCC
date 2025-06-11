using GSG.NET.Concurrent;
using GSG.NET.Excel;
using GSG.NET.FileSystem;
using GSG.NET.LINQ;
using GSG.NET.Logging;
using GSG.NET.PLC.Mitsubishi;
using GSG.NET.Utils;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Enum;
using VASFx.MLCC.Common.Events;
using VASFx.MLCC.Common.Model;
using VASFx.MLCC.Common.VisionModel;
using VASFx.MLCC.Data.Sqlite;
using VASFx.MLCC.Data.Sqlite.Entity;
using VASFx.MLCC.VisionLibrary.Cognex;

namespace VASFx.MLCC.Core
{
    public abstract class CoreBase : ICore, IDisposable
    {
        protected Logger logger = Logger.GetLogger();

        protected Dictionary<string, Thread> dicTh = new Dictionary<string, Thread>();
        protected IList<WorkerItem> workers = new List<WorkerItem>();

        protected Dictionary<eCamID, ICognexLib> dicCams = new Dictionary<eCamID, ICognexLib>();

        protected IEventAggregator eventAggregator = null;
        protected IContainerProvider provider = null;
        protected MizManager plc = null;
        protected SqlManager sql = null;
        protected ViewLoggerGenerator viewLoggerGenerator = null;

        protected eSystemState SystemState = eSystemState.None;

        public CoreBase(IEventAggregator eventAggregator, IContainerProvider provider, MizManager plc, SqlManager sql, ViewLoggerGenerator viewLogger, CogCamManager camManager)
        {
            this.plc = plc;
            this.eventAggregator = eventAggregator;
            this.sql = sql;
            this.provider = provider;
            this.viewLoggerGenerator = viewLogger;
            this.viewLoggerGenerator.Init();

            this.dicCams = camManager.CamsDic;

            eventAggregator.GetEvent<GUIMessagePubSubEvent>().Subscribe(UICallBackCommand, ThreadOption.BackgroundThread);

            GSG.NET.Utils.MidnightNotifier.DayChanged += MidnightNotifier_DayChanged;
            GSG.NET.Utils.FixTimeNotifier.HourChanged += FixTimeNotifier_HourChanged;
        }

        #region UI Call Back Command
        private void UICallBackCommand(GUIEventArgs obj)
        {
            switch (obj.Kind)
            {
                case eVASFxEventKind.SystemStateChange:
                    ReqSystemStateChange(obj);
                    break;
                case eVASFxEventKind.ModelChange:
                    ReqModelChange();
                    break;
                case eVASFxEventKind.CalibrationComplete:
                    break;
                case eVASFxEventKind.AddedAlignHistory:
                    break;
                case eVASFxEventKind.AddedInspectionHistory:
                    break;
                default:
                    break;
            }
        }

        protected virtual void ReqTest() { }

        private void ReqModelChange()
        {
            var model = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;
            var currentModelName = model.Name;
            dicCams.Values.ToList().ForEach(cam =>
            {
                cam.LoadModel(currentModelName);
            });

            plc.WriteWord("TO_VISION_MODEL_NUMBER", model.ModelID.ToString());
        }

        protected abstract void ReqSystemStateChange(GUIEventArgs obj);

        #endregion

        public abstract void Init();

        public void InitPLC(string fileName)
        {
             var plcConfig = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + fileName).Fetch<PLCConfig>().ToList().FirstOrDefault();

            var grpB = new MizGroup { Name = "B1", Device = MizDevice.M };
            var grpW = new MizGroup { Name = "W1", Device = MizDevice.D };

            Assert.IsTrue(MakeBitMap(grpB, fileName), "PLC Bit Map Load Fail");
            Assert.IsTrue(MakeWordMap(grpW, fileName), "PLC Word Map Load Fail");

            this.plc.Config = new MizConfig()
            {
                IpAddress = plcConfig.Addr,
                Port = plcConfig.PortNo,
                RollingCount = 5,
                Id = plcConfig.Name,
                MonitorInterval = 50,
                IsAsciiComn = plcConfig.IsAscii,
            };
            this.plc.DelayAutoOff = 700;

            this.plc.AddGroup(grpB);
            this.plc.AddGroup(grpW);

            //Event Connection
            this.plc.OnConnect += Plc_OnConnect;
            this.plc.OnDisconnect += Plc_OnDisconnect;
            this.plc.OnFirstColtd += Plc_OnFirstColtd;
            this.plc.OnCollected += Plc_OnCollected;
            this.plc.OnBitChanged += Plc_OnBitChanged;
            this.plc.OnWordChanged += Plc_OnWordChanged;
            this.plc.OnLog += Plc_OnLog;
            this.plc.HeartBeat("TO_VISION_ALIVE", 3000);

            //Connect UI 표시를 위해 (5sec Deley)
            GSG.NET.Quartz.TimerUtils.Once(5000, () => { this.plc.Connect(); });
        }

        bool MakeWordMap(MizGroup grpW, string path)
        {
            var ll = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + path).Fetch<XlsW>("W1");
            if (ll == null || !ll.Any())
                return false;

            ll = ll.Where(x => !string.IsNullOrEmpty(x.TagName)).ToList();

            foreach (var item in ll)
            {
                grpW.AddWordBlock(new MizWordBlock
                {
                    Name = item.TagName,
                    Address = item.Addr,
                    SubText = item.SubText,
                    SubNo = item.SubNo,
                    Point = item.Point,
                    Format = item.Format,
                    Multiple = item.MultipleV,
                    MultipleFormatter = item.MultipleFormat,
                    IsWatch = item.Watch,
                    KindE = item.Kind,
                    CallbackOrder = item.CallbackOrder,
                    Comment = item.Comment,
                    BitType = item.BitType,
                });
            }

            return true;
        }

        bool MakeBitMap(MizGroup grpB, string path)
        {
            var ll = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + path).Fetch<XlsB>("B1");
            if (ll == null || !(ll.Count() > 0))
                return false;

            ll = ll.Where(x => !string.IsNullOrEmpty(x.TagName)).ToList();

            foreach (var item in ll)
            {
                grpB.AddBitBlock(new MizBitBlock
                {
                    Name = item.TagName,
                    Address = item.Addr,
                    KindE = item.Kind,
                    SubText = item.SubText,
                    SubNo = item.SubNo,
                    CallbackOrder = item.CallbackOrder,
                    Comment = item.Comment,
                    IsAutoOff = item.AutoOff,
                });
            }

            return true;
        }

        private void FixTimeNotifier_HourChanged(object sender, EventArgs e)
        {
            try
            {
                var setDay = sql.SystemSetting.FindBy(x => x.Name.Equals("ImageBackupDay")).Single().Value.FwFloatOf();
                var dt = DateTime.Now.AddDays(-setDay);
                var directoryPath = sql.SystemSetting.FindBy(x => x.Name.Equals("ImageSavePath")).Single().Value;

                int delCount = 0;

                foreach (string path in FileUtils.GetFiles(directoryPath, "*", true))
                {
                    var fi = new FileInfo(path);
                    if (fi.LastWriteTime > dt)
                        continue;

                    FileUtils.DeleteFileIfExist(path);
                    delCount++;
                    logger.D($"Deleted File - Name : [ {fi.Name} ]");

                    if (delCount > 200)
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.E(ex);
            }
        }

        private void MidnightNotifier_DayChanged(object sender, EventArgs e)
        {
            try
            {
                //History Clean.
                var setDay = sql.SystemSetting.FindBy(x => x.Name.Equals("LogBackupDay")).Single().Value.FwFloatOf();
                var backup = DateTime.Now.AddDays(-setDay);
                this.sql.AlignHistory.Delete(x => x.CreateDate < backup);
                this.sql.InspectionHistory.Delete(x => x.CreateDate < backup);

                //DB Backup
                var backupDir = @"D:\DB\Backup\";
                var backupFileName = $@"Backup_{DateTime.Now.ToString("yy/MM/dd")}.sqlite";
                //var sourcePath = @"D:\DB\VASFxDb.sqlite";

                var connectionString = ConfigurationManager.ConnectionStrings[sql.ConnectStringName].ConnectionString;
                var startIndex = connectionString.IndexOf("=");
                var endIndex = connectionString.IndexOf(";");
                var sourcePath = connectionString.Substring(startIndex + 1, (endIndex - startIndex) - 1);

                var targetFullPath = System.IO.Path.Combine(backupDir + backupFileName);

                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                System.IO.File.Copy(sourcePath, targetFullPath);

                //DB Backup Clean
                int delCount = 0;
                foreach (string path in FileUtils.GetFiles(backupDir, "*", false))
                {
                    var fi = new FileInfo(path);
                    if (fi.LastWriteTime > backup)
                        continue;

                    FileUtils.DeleteFileIfExist(path);
                    delCount++;
                    logger.D($"Deleted File - Name : [ {fi.Name} ]");

                    if (delCount > 100)
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.E(ex);
            }
        }

        /// <summary>
        /// Align Log DB Add
        /// UI에 DB UpDate 를 알리기 위해서 Message Publish.
        /// </summary>
        /// <param name="log"></param>
        protected void AddAlignLogPublish(AlignHistory log)
        {
            this.sql.AlignHistory.Add(log);

            var msg = new CoreEventArgs() { Kind = eVASFxEventKind.AddedAlignHistory };
            msg.Arg = log;
            this.eventAggregator.GetEvent<CoreMessagePubSubEvent>().Publish(msg);
        }

        /// <summary>
        /// Inspection Log DB Add
        /// UI에 DB UpDate 를 알리기 위해서 Message Publish.
        /// </summary>
        /// <param name="log"></param>
        protected void AddInspectionLogPublish(InspectionHistory log)
        {
            this.sql.InspectionHistory.Add(log);

            var msg = new CoreEventArgs() { Kind = eVASFxEventKind.AddedInspectionHistory };
            msg.Arg = log;
            this.eventAggregator.GetEvent<CoreMessagePubSubEvent>().Publish(msg);
        }


        #region PLC Events
        private void Plc_OnCollected(GSG.NET.PLC.Support.MapScan scan)
        {
        }

        private void Plc_OnLog(string log)
        {
            logger.I(log);
        }

        protected abstract void Plc_OnWordChanged(GSG.NET.PLC.Model.WordBlock block);

        protected abstract void Plc_OnBitChanged(GSG.NET.PLC.Model.BitBlock block);

        private void Plc_OnFirstColtd(GSG.NET.PLC.PFConfig cfg)
        {
            logger.I("PLC First Collected");
        }

        private void Plc_OnDisconnect(string id)
        {
            logger.I("PLC Disconnected");
        }

        private void Plc_OnConnect(string id)
        {
            logger.I("PLC Connected");

            var currentModel = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;
            plc.WriteWord("TO_VISION_MODEL_NUMBER", currentModel.ModelID);

            ResetPLCInterfaceBit();
        }

        protected abstract void ResetPLCInterfaceBit();

        protected virtual void SetVisionAlarm(string Tag, eVisionAlarm alarm)
        {
            var alarmWord = plc.ReadWord(Tag);
            var curr = BitUtils.ChgBitArray(alarmWord.IntValue);
            curr[(int)alarm] = true;

            int writeValue = BitUtils.ChgInt32(curr);
            plc.WriteWord(Tag, writeValue);
        }

        protected virtual void ResetVisionAlarm(string Tag, eVisionAlarm alarm)
        {
            var alarmWord = plc.ReadWord(Tag);
            var curr = BitUtils.ChgBitArray(alarmWord.IntValue);
            curr[(int)alarm] = false;

            int writeValue = BitUtils.ChgInt32(curr);
            plc.WriteWord(Tag, writeValue);
        }

        protected virtual void ResetZoneVisionAlarm(string Tag)
        {
            plc.WriteWord(Tag, 0);
        }

        #endregion

        #region Thread Method
        protected bool IsWokerProcessing() => workers.Any(w => w.IsProcessing);

        protected void AddWorker(WorkerItem item)
        {
            this.workers.Add(item);
            this.dicTh.Add(item.Name, ThreadUtils.Invoke<WorkerItem>(DoWork, item));
        }

        void DoWork(WorkerItem item)
        {
            Thread.CurrentThread.Name = $"{this.GetType().Name} - {item.Name}";
            logger.D($"Core - Process Started {Thread.CurrentThread.Name}");

            while (true)
            {
                try
                {
                    item.IsProcessing = false;
                    var q = item.Queue.Dequeue();
                    item.IsProcessing = true;

                    switch (q)
                    {
                        case eVASFxCoreCommandKind.Align1st:
                            item.ResetFirstAlignResult();
                            item.Align1st(item.FirstAlignResult);
                            break;
                        case eVASFxCoreCommandKind.Align2nd:
                            item.Align2nd(item.FirstAlignResult);
                            break;
                        case eVASFxCoreCommandKind.Calibration1st:
                            item.Calibration1st();
                            break;
                        case eVASFxCoreCommandKind.Calibration2nd:
                            item.Calibration2nd();
                            break;
                        case eVASFxCoreCommandKind.Inspection1st:
                            item.Inspection1st();
                            break;
                        case eVASFxCoreCommandKind.Inspection2nd:
                            item.Inspection2nd();
                            break;
                        default:
                            break;
                    }

                }
                catch (ThreadInterruptedException e)
                {
                    logger.D($"Core - Thread Interrupted - {Thread.CurrentThread.Name} : {e}");
                    break;
                }
                catch (ThreadAbortException e)
                {
                    logger.D($"Core - Thread Abort - {Thread.CurrentThread.Name} : {e}");
                    break;
                }
                catch (Exception e)
                {
                    logger.E(e);
                }
            }

            logger.D($"Core - Process Disposed - {Thread.CurrentThread.Name}");
        }
        #endregion

        protected virtual void ExecuteModelChange()
        {
            plc.WriteBit("TO_MODEL_CHG_START", true);

            var targetID = plc.ReadWord("FR_MODEL_NUMBER").DoubleValue;

            try
            {
                var targetModel = sql.ProductModel.FindBy(x => x.ModelID == targetID).SingleOrDefault();
                var modelID = sql.SystemInfo.GetAll().FirstOrDefault().CurrentProductModelId;
                var currentModel = sql.ProductModel.FindBy(x => x.Id == modelID).SingleOrDefault();

                if (targetModel == null)
                {
                    plc.WriteBit("TO_MODEL_CHG_FAIL", true);
                    viewLoggerGenerator.Publish("Model Change Fail - Request Model ID not Exist", eViewLoggerArgsKind.PLC, eViewLoggerArgsLevelKind.Fail);
                    return;
                }

                sql.SystemInfo.GetAll().FirstOrDefault().CurrentProductModelId = targetModel.Id;
                plc.WriteWord("TO_CURRENT_MODEL", targetID.ToString());
                LockUtils.Wait(500);
                plc.WriteBit("TO_MODEL_CHG_OK", true);

                this.viewLoggerGenerator.Publish($"Model Changed {currentModel.Name} -> {targetModel.Name}", eViewLoggerArgsKind.PLC, eViewLoggerArgsLevelKind.Info);

                ReqModelChange();//Laod Cam Model
            }
            catch (Exception e)
            {
                this.viewLoggerGenerator.Publish($"Model Changed Fail - Exception", eViewLoggerArgsKind.PLC, eViewLoggerArgsLevelKind.Fail);
                plc.WriteBit("TO_MODEL_CHG_FAIL", true);
                logger.E(e);
            }

            var msg = new CoreEventArgs() { Kind = eVASFxEventKind.ModelChange };
            this.eventAggregator.GetEvent<CoreMessagePubSubEvent>().Publish(msg);
        }

        public virtual void Dispose()
        {
            this.dicTh.Values.ToList().ForEach(th =>
            {
                if (!th.Stop())
                {
                    ThreadUtils.Kill(th);
                }
            });

            this.dicCams.Values.ToList().ForEach(cam =>
            {
                cam.DisconnectCamera();
                //logger.I($"{cam.Config.ID} - Disconnected");
            });
        }
    }


    public class WorkerItem
    {
        public string Name { get; set; }
        public TsQueue<eVASFxCoreCommandKind> Queue { get; set; }
        public bool IsProcessing { get; set; }
        public Action<ZoneFirstAlignResult> Align1st { get; set; }
        public Action<ZoneFirstAlignResult> Align2nd { get; set; }
        public Action Calibration1st { get; set; }
        public Action Calibration2nd { get; set; }
        public Action Inspection1st { get; set; }
        public Action Inspection2nd { get; set; }

        public ZoneFirstAlignResult FirstAlignResult { get; set; } = new ZoneFirstAlignResult(false);

        public void ResetFirstAlignResult()
        {
            this.FirstAlignResult = null;
            this.FirstAlignResult = new ZoneFirstAlignResult(false);
        }

        public WorkerItem(string name)
        {
            IsProcessing = false;
            Name = name;
        }
    }

    public class ZoneFirstAlignResult
    {
        public bool IsSuccess { get; set; }
        public XY OneQuarter { get; set; }
        public XY TwoQuarter { get; set; }
        public XY ThreeQuarter { get; set; }
        public XY FourQuarter { get; set; }

        public ZoneFirstAlignResult(bool isSuccess)
        {
            this.IsSuccess = isSuccess;

            OneQuarter = new XY();
            TwoQuarter = new XY();
            ThreeQuarter = new XY();
            FourQuarter = new XY();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
