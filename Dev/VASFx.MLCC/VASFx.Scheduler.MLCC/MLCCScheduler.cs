using GSG.NET.Concurrent;
using GSG.NET.Logging;
using GSG.NET.PLC.Mitsubishi;
using GSG.NET.PLC.Model;
using GSG.NET.PLC.SLMP;
using GSG.NET.Utils;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Events;
using VASFx.Common.HistoryModel;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.Core;
using VASFx.InspectionFormulas.MeshAvailability;
using VASFx.MLCC.Sqlite;

namespace VASFx.Scheduler.MLCC
{
    public class MLCCScheduler : CoreBase
    {
        Logger pioLogger = Logger.GetLogger("PIO");
        Logger Logger = Logger.GetLogger();
        AlgorithmProcesser algorithmProcesser = null;

        TsQueue<eVASFxCoreCommandKind> meshInspection = new TsQueue<eVASFxCoreCommandKind>();

        public MLCCScheduler(IEventAggregator eventAggregator, IContainerProvider provider, SlmpManager plc, SqlManager sql, CameraManager cameraManager, LightControlManager lightControlManager)
            : base(eventAggregator, provider, plc, sql, cameraManager, lightControlManager)
        {
            eventAggregator.GetEvent<ApplicationExitEvent>().Subscribe((o) => Dispose(), true);

            this.algorithmProcesser = new AlgorithmProcesser(sql, cameraManager, lightControlManager);
        }

        public override void Init()
        {
            var u1 = new CoreWorker("U1");
            u1.WorkQueue = this.meshInspection;
            u1.Inspection1st = U1_InspectionAction;
            base.Addworker(u1);

            base.Init();
        }
        protected override void Plc_OnBitChanged(BitBlock block)
        {
            if (block.Name.Equals("FR_PLC_ALIVE")) return;

            pioLogger.I(block);

            try
            {
                if (!block.IsBitOn)
                    return;

                CoreEventArgs msg = new CoreEventArgs();

                switch (block.Name)
                {
                    case "FR_MLCC_INSPECTION_START":
                        this.meshInspection.Enqueue(eVASFxCoreCommandKind.Inspection1st);

                        msg.MessageKind = eCoreMessageKind.ViewLoggerChanged;
                        msg.MessageHeader = "PLC";
                        msg.MessageKey = "INFO";
                        msg.MessageText = "Mesh Inspection Start Command Enqueue";

                        _coreEventPulisher.Publish(msg);
                        break;

                    case "FR_ERROR_RESET":
                        msg.MessageKind = eCoreMessageKind.ViewLoggerChanged;
                        msg.MessageHeader = "PLC";
                        msg.MessageKey = "INFO";
                        msg.MessageText = "PLC Error Reset Bit On";
                        _coreEventPulisher.Publish(msg);

                        this.ResetPLCInterfaceBit();
                        break;

                    case "FR_REQ_MODEL_CHANGE":
                        msg.MessageKind = eCoreMessageKind.ViewLoggerChanged;
                        msg.MessageHeader = "PLC";
                        msg.MessageKey = "INFO";
                        msg.MessageText = "Model Change Request";
                        _coreEventPulisher.Publish(msg);

                        this.ExecuteModelChange();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.E(ex.StackTrace);
            }
        }

        protected override void Plc_OnWordChanged(WordBlock block)
        {
        }

        protected override void ResetPLCInterfaceBit()
        {
            if (this.IsWokerProcessing) return;
     
            plc.WriteBit("TO_VISION_MLCC_INSPECTION_READY", true);
            plc.WriteBit("TO_VISION_AUTO_MODE", true);

            plc.WriteBit("TO_MLCC_INSPECTION_START", false);
            plc.WriteBit("TO_MLCC_INSPECTION_END", false);
            plc.WriteBit("TO_MLCC_INSPECTION_OK", false);
            plc.WriteBit("TO_MLCC_INSPECTION_NG", false);

            //TODO: Zone1, 2, 3, 4 Error Bit
            //plc.WriteBit("TO_VISION_ZONE1_ERROR", false);
            //plc.WriteBit("TO_VISION_ZONE2_ERROR", false);  
            //plc.WriteBit("TO_VISION_ZONE3_ERROR", false);

            //TODO: Alarm Bits Off
            plc.WriteBit("TO_VISION_LIGHT_ON_ERR", false);
            plc.WriteBit("TO_VISION_CONNECT_CAMERA_ERR", false);

            // Inspection Result Bits Off
            plc.WriteBit("TO_MESH1_EXISTENCE", false);
            plc.WriteBit("TO_MESH2_EXISTENCE", false);
            plc.WriteBit("TO_MESH3_EXISTENCE", false);
            plc.WriteBit("TO_MESH4_EXISTENCE", false);
            plc.WriteBit("TO_MESH5_EXISTENCE", false);
            plc.WriteBit("TO_MESH6_EXISTENCE", false);

            plc.WriteBit("TO_MESH1_OVER_LAP", false);
            plc.WriteBit("TO_MESH2_OVER_LAP", false);
            plc.WriteBit("TO_MESH3_OVER_LAP", false);
            plc.WriteBit("TO_MESH4_OVER_LAP", false);
            plc.WriteBit("TO_MESH5_OVER_LAP", false);
            plc.WriteBit("TO_MESH6_OVER_LAP", false);

        }

        protected override void RunProcesser(GUIEventArgs obj)
        {
            var camID = obj.ProcessPosition.CamID;
            var zoneID = obj.ProcessPosition.ZoneID;
            var grabPosition = obj.ProcessPosition.GrabPosition;
            var loadImage = obj.LoadImage;

            //RunProcesser
            var processResult = this.algorithmProcesser.RunMeshAvailability(zoneID, grabPosition, camID, loadImage);

            //Publish result
            var resultArgs = new CoreEventArgs();
            resultArgs.MessageKind = eCoreMessageKind.ResultChange;
            resultArgs.Arg = processResult;
            this._coreEventPulisher.Publish(resultArgs);
        }

        #region U1 Action

        private void U1_InspectionAction()
        {
            //if (this._systemState != eSystemState.Auto)
            //    return;

            long sT = SwUtils.CurrentTimeMillis;

            SetStartBitInspection(eExecuteZone.MLCC_INSPECTION); //Word 정리 후 Bit On

            var processResult = algorithmProcesser.RunMeshAvailability(eExecuteZone.MLCC_INSPECTION, eGrabPosition.First, eCamID.Cam1);

            if (processResult.Alarm != eVisionAlarm.None)
            {
                switch (processResult.Alarm)
                {
                    case eVisionAlarm.FailLightOn:
                        logger.E($"Scheduler Light on err");
                        ResetPLCInterfaceBit();
                        SetProcessedResult(eExecuteZone.MLCC_INSPECTION, new ResultMLCCInspection(0), $"MLCC LightOn Fail", false, processResult.Alarm);
                        break;
                    case eVisionAlarm.DisconnectedCamera:
                        logger.E($"Scheduler Camera grab err");
                        ResetPLCInterfaceBit();
                        SetProcessedResult(eExecuteZone.MLCC_INSPECTION, new ResultMLCCInspection(0), $"MLCC GrabImage Fail", false, processResult.Alarm);
                        break;
                }
            }
            else
            {
                //mlcc inspection result
                var resultArgs = new CoreEventArgs();
                resultArgs.MessageKind = eCoreMessageKind.ResultChange;
                resultArgs.Arg = processResult;
                this._coreEventPulisher.Publish(resultArgs);

                SetProcessedResult(eExecuteZone.MLCC_INSPECTION, processResult, $"MLCC Inspection Success");
            }

        }

        #endregion

        #region Common

        void SetStartBitInspection(eExecuteZone zone)
        {
            if (zone != eExecuteZone.MLCC_INSPECTION)
                logger.E("SetStartBit Method parameter not Define Case");

            plc.WriteBit("TO_MESH1_EXISTENCE", false);
            plc.WriteBit("TO_MESH2_EXISTENCE", false);
            plc.WriteBit("TO_MESH3_EXISTENCE", false);
            plc.WriteBit("TO_MESH4_EXISTENCE", false);
            plc.WriteBit("TO_MESH5_EXISTENCE", false);
            plc.WriteBit("TO_MESH6_EXISTENCE", false);

            plc.WriteBit("TO_MESH1_OVER_LAP", false);
            plc.WriteBit("TO_MESH2_OVER_LAP", false);
            plc.WriteBit("TO_MESH3_OVER_LAP", false);
            plc.WriteBit("TO_MESH4_OVER_LAP", false);
            plc.WriteBit("TO_MESH5_OVER_LAP", false);
            plc.WriteBit("TO_MESH6_OVER_LAP", false);

            plc.WriteBit($"TO_VISION_{zone}_READY", false);

            plc.WriteBit($"TO_{zone}_START", true);

            pioLogger.I($"{zone} - Set Start Bit");
        }

        bool SetEndBitInspection(eExecuteZone zone, bool isSuccess)
        {
            try
            {
                if (zone != eExecuteZone.MLCC_INSPECTION)
                    logger.E("SetEndBitAlign Method parameter not Define Case");

                plc.WriteBit($"TO_{zone}_START", false);

                if (isSuccess)
                    plc.WriteBit($"TO_{zone}_OK", true);
                else
                    plc.WriteBit($"TO_{zone}_NG", true);

                plc.WriteBit($"TO_{zone}_END", true);
                pioLogger.E($"Write TO_{zone}_END True Start- Debugging ");

                LockUtils.Wait(500);

                pioLogger.E($"Write TO_{zone}_END True End- Debugging ");

                //if (!plc.WaitChgBit(true, 5000, $"TO_{zone}_END"))
                //{
                //    pioLogger.E($"TO_{zone}_END Time Over");
                //    plc.WriteBit($"TO_{zone}_END", true);

                //    LockUtils.Wait(1800);
                //}

                if (!plc.WaitChgBit(true, 10000, $"FR_{zone}_END")) 
                {
                    plc.WriteBit($"TO_{zone}_END", true);
                    LockUtils.Wait(1800);

                    pioLogger.E($"FR_{zone}_END Time Over - Debugging");
                }

                plc.WriteBit($"TO_{zone}_OK", false);
                plc.WriteBit($"TO_{zone}_NG", false);

                plc.WriteBit($"TO_{zone}_END", false);
                plc.WriteBit($"TO_VISION_{zone}_READY", true);

                pioLogger.I($"{zone} - Set Align End Bit");

                return true;
            }
            catch (Exception ex)
            {
                logger.D($"{ex} - End process exception");
                return false;
            }

        }
        void SetAlarmBitInspection(eVisionAlarm alarm)
        {
            switch (alarm)
            {
                case eVisionAlarm.FailLightOn:
                    plc.WriteBit("TO_VISION_LIGHT_ON_ERR", true);
                    break;

                case eVisionAlarm.DisconnectedCamera:
                    plc.WriteBit("TO_VISION_CONNECT_CAMERA_ERR", true);
                    break;
            }
        }
        void SetProcessedResult(eExecuteZone zone, ResultMLCCInspection result, string reason, bool isSuccess = true, eVisionAlarm alarm = eVisionAlarm.None)
        {
            InspectionHistory his = new InspectionHistory();
            his.Zone = zone;
            his.CreateDate = DateTime.Now;

            result.ResultMLCCItems.OrderBy(x => x.MeshID);
                       
            var levelKind = "INFO";
            var subKind = "INSPECTION";

            if (isSuccess)
            {

                for (int i = 0; i < result.ResultMLCCItems.Count; i++)
                {
                    his.MeshId = result.ResultMLCCItems[i].MeshID;
                    his.Area = result.ResultMLCCItems[i].Area;
                    his.IsExistence = result.ResultMLCCItems[i].IsExistence;
                    his.IsOverlap = result.ResultMLCCItems[i].IsOverlap;

                    SetResultBit(his);

                    AddViewLogPublish(his, reason, subKind, levelKind);
                }

                var ret = SetEndBitInspection(zone, isSuccess);

                //retry
                if(!ret) SetEndBitInspection(zone, isSuccess);
            }
            else
            {
                levelKind = "FAIL";

                SetAlarmBitInspection(alarm);

                AddViewLogPublish(his, reason, subKind, levelKind);

                var ret = SetEndBitInspection(zone, isSuccess);

                //retry
                if (!ret) SetEndBitInspection(zone, isSuccess);
            }

        }
        void SetResultBit(InspectionHistory log)
        {
            var isExistecne = log.IsExistence.Equals(true) ? true : false;
            var isOverlap = log.IsOverlap.Equals(true) ? true : false;

            plc.WriteBit($"TO_{log.MeshId}_EXISTENCE", isExistecne);
            plc.WriteBit($"TO_{log.MeshId}_OVER_LAP", isOverlap);

        }
        void AddViewLogPublish(InspectionHistory log, string reason, String subKind, String levelKind)
        {
            if (levelKind == "INFO")
            {
                this.AddInspectionLogPublish(log);

                CoreEventArgs msg = new CoreEventArgs();

                msg.MessageKind = eCoreMessageKind.ViewLoggerChanged;
                msg.MessageHeader = subKind;
                msg.MessageKey = levelKind;
                msg.MessageText = $"{log.MeshId} / {log.Area} / IsExtence:{log.IsExistence} / IsOverRap:{log.IsOverlap} ]";
                _coreEventPulisher.Publish(msg);

                this.pioLogger.I($"{log.MeshId} / {log.Area} / IsExtence:{log.IsExistence} / IsOverRap:{log.IsOverlap} ]");
            }
            else
            {
                CoreEventArgs msg = new CoreEventArgs();

                msg.MessageKind = eCoreMessageKind.ViewLoggerChanged;
                msg.MessageHeader = subKind;
                msg.MessageKey = levelKind;
                msg.MessageText = $"[{reason} ]";
                _coreEventPulisher.Publish(msg);
            }

        }
        #endregion
    }
}
