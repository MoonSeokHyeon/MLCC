using GSG.NET.Excel;
using GSG.NET.FileSystem;
using GSG.NET.Logging;
using GSG.NET.PLC;
using GSG.NET.PLC.SLMP;
using GSG.NET.Utils;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using VASFx.Common.Events;
using VASFx.Common.HistoryModel;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.MLCC.Sqlite;

namespace VASFx.Core
{
    public abstract class CoreBase : IDisposable
    {
        protected static Logger logger = Logger.GetLogger();

        protected IList<CoreWorker> workers = new List<CoreWorker>();

        protected IEventAggregator eventAggregator = null;
        protected IContainerProvider containerProvider = null;
        protected SlmpManager plc = null;
        protected SqlManager sql = null;
        protected CameraManager cameraManager = null;
        protected LightControlManager lightControlManager = null;
        protected eSystemState _systemState = eSystemState.Auto;

        protected CoreMessageEvent _coreEventPulisher = null;

        protected bool IsWokerProcessing => workers.Any(w => w.IsProcessing);

        #region Construct
        public CoreBase(IEventAggregator eventAggregator, IContainerProvider containerProvider, SlmpManager plc, SqlManager sql, CameraManager cameraManager, LightControlManager lightControlManager)
        {
            this.eventAggregator = eventAggregator;
            this.containerProvider = containerProvider;
            this.plc = plc;
            this.sql = sql;
            this.cameraManager = cameraManager;
            this.lightControlManager = lightControlManager;

            this._coreEventPulisher = this.eventAggregator.GetEvent<CoreMessageEvent>();

            this.eventAggregator.GetEvent<GUIMessageEvent>().Unsubscribe(OnReceivedMessage);
            this.eventAggregator.GetEvent<GUIMessageEvent>().Subscribe(OnReceivedMessage, ThreadOption.BackgroundThread);

            GSG.NET.Utils.MidnightNotifier.DayChanged += MidnightNotifier_DayChanged;
            GSG.NET.Utils.FixTimeNotifier.HourChanged += FixTimeNotifier_HourChanged;
        }

        public void Dispose()
        {
            this.workers.ToList().ForEach(worker => worker.Dispose());
            this.plc.Disconnect();
        }

        public virtual void Init()
        {
            InitPLC();
        }
        #endregion

        #region Abstract Mathod.
        protected abstract void Plc_OnWordChanged(GSG.NET.PLC.Model.WordBlock block);

        protected abstract void Plc_OnBitChanged(GSG.NET.PLC.Model.BitBlock block);

        protected abstract void ResetPLCInterfaceBit();

        protected abstract void RunProcesser(GUIEventArgs obj);

        #endregion

        #region UI Communication

        private void OnReceivedMessage(GUIEventArgs obj)
        {
            switch (obj.MessageKind)
            {
                case Common.Shared.eGUIMessageKind.SystemStateChange:
                    this.RspSystemStateChange(obj);
                    break;
                case Common.Shared.eGUIMessageKind.ModelChange:
                    this.ReqModelChange(obj);
                    break;
                case Common.Shared.eGUIMessageKind.ChangeModelList:
                    break;
                case Common.Shared.eGUIMessageKind.LightValueChange:
                    break;
                case Common.Shared.eGUIMessageKind.Calibration:
                    break;
                case Common.Shared.eGUIMessageKind.CalibrationComplete:
                    break;
                case Common.Shared.eGUIMessageKind.AddedAlignHistory:
                    break;
                case Common.Shared.eGUIMessageKind.AddedInspectionHistory:
                    break;
                case Common.Shared.eGUIMessageKind.FailModelChange:
                    break;
                case Common.Shared.eGUIMessageKind.CameraPropertyChanged:
                    break;
                case Common.Shared.eGUIMessageKind.MainViewChanged:
                    break;
                case Common.Shared.eGUIMessageKind.RunProcesser:
                    this.RunProcesser(obj);
                    break;
                default:
                    break;
            }
        }

        void RspSystemStateChange(GUIEventArgs args)
        {
            var reqState = (eSystemState)args.Arg;
            this._systemState = reqState;

            if (this._systemState == eSystemState.Auto)
                plc.WriteBit("TO_VISION_AUTO_MODE", true);

            else
                plc.WriteBit("TO_VISION_AUTO_MODE", false);

            ResetPLCInterfaceBit();

            var msg = new CoreEventArgs();
            msg.MessageKind = eCoreMessageKind.SystemStateChange;
            msg.Arg = this._systemState;
            _coreEventPulisher.Publish(msg);
        }
        void ReqModelChange(GUIEventArgs args)
        {
            var modelName = args.Arg.ToString();

            cameraManager.ModelChangeJobs(modelName);
        }
        #endregion

        #region Model Change
        protected virtual void ExecuteModelChange()
        {

        }
        #endregion

        #region PLC Event & PLC Init
        void InitPLC()
        {
            var fileName = ConfigurationManager.AppSettings["PLCMAP_FILE"];
            var plcConfig = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + fileName).Fetch<PLCConfig>().ToList().FirstOrDefault();

            var grpB = new SlmpGroup { Name = "B1", Device = SlmpDevice.M };
            var grpW = new SlmpGroup { Name = "W1", Device = SlmpDevice.D };

            Assert.IsTrue(MakeBitMap(grpB, fileName), "PLC Bit Map Load Fail");
            Assert.IsTrue(MakeWordMap(grpW, fileName), "PLC Word Map Load Fail");

            this.plc.Config = new SlmpConfig()
            {
                IpAddress = plcConfig.Addr,
                Port = plcConfig.PortNo,
                RollingCount = 5,
                Id = plcConfig.Name,
                MonitorInterval = 50,
                //IsAsciiComn = plcConfig.IsAscii,
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
        private void Plc_OnConnect(string id)
        {
            logger.I("PLC Connected");

            var currentModel = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;
            plc.WriteWord("TO_VISION_MODEL_NUMBER", currentModel.ModelId);
            plc.WriteBit("TO_VISION_AUTO_MODE", true);
            //ResetPLCInterfaceBit();
        }

        private void Plc_OnDisconnect(string id)
        {
            logger.I("PLC Disconnected");
        }

        protected void Plc_OnFirstColtd(PFConfig cfg)
        {
            logger.I("PLC First Collected");

        }
        private void Plc_OnCollected(GSG.NET.PLC.Support.MapScan scan)
        {
        }
        private void Plc_OnLog(string log)
        {
            logger.I(log);
        }

        bool MakeWordMap(SlmpGroup grpW, string path)
        {
            var ll = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + path).Fetch<XlsW>("W1");
            if (ll == null || !ll.Any())
                return false;

            ll = ll.Where(x => !string.IsNullOrEmpty(x.TagName)).ToList();

            foreach (var item in ll)
            {
                grpW.AddWordBlock(new SlmpWordBlock
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

        bool MakeBitMap(SlmpGroup grpB, string path)
        {
            var ll = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + path).Fetch<XlsB>("B1");
            if (ll == null || !(ll.Count() > 0))
                return false;

            ll = ll.Where(x => !string.IsNullOrEmpty(x.TagName)).ToList();

            foreach (var item in ll)
            {
                grpB.AddBitBlock(new SlmpBitBlock
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
        #endregion

        #region Protected Method
        protected void Addworker(CoreWorker worker)
        {
            this.workers.Add(worker);
        }

        protected CoreWorker GetCoreWorker(string name)
        {
            return workers.FirstOrDefault(_ => _.Name.Equals(name));
        }
        protected void AddInspectionLogPublish(InspectionHistory log)
        {
            this.sql.InspectionHistory.Add(log);

            var msg = new CoreEventArgs() { MessageKind = eCoreMessageKind.AddedInspectionHistory };
            msg.Arg = log;
            this.eventAggregator.GetEvent<CoreMessageEvent>().Publish(msg);
        }

        #endregion

        #region Private Method

        private void FixTimeNotifier_HourChanged(object sender, EventArgs e)
        {
            try
            {
                var setDay = double.Parse(sql.SystemSetting.FindBy(x => x.Name.Equals("ImageBackUpDays")).Single().Value); ;
                var dt = DateTime.Now.AddDays(-setDay);
                var directoryPath = ConstLogString.ImageBackUpPath;

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
                var setDay = double.Parse(sql.SystemSetting.FindBy(x => x.Name.Equals("LogBackUpDays")).Single().Value);
                var backup = DateTime.Now.AddDays(-setDay);
                //this.sql.AlignHistory.Delete(x => x.CreateDate < backup);
                this.sql.InspectionHistory.Delete(x => x.CreateDate < backup);

                //DB Backup
                var backupDir = @"D:\DB\Backup\";
                var backupFileName = $@"Backup_{DateTime.Now.ToString("yy/MM/dd")}.sqlite";
                //var sourcePath = @"D:\DB\VASFxDb.sqlite";

                var connectionString = ConfigurationManager.ConnectionStrings[sql.ConnentionString].ConnectionString;
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
        #endregion
    }
}
