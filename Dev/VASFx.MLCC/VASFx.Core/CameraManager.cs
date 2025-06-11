using GSG.NET.Extensions;
using GSG.NET.Utils;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Events;
using VASFx.Common.Interface;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.Device.Camera.Cameras;
using VASFx.MLCC.Sqlite;
using VASFx.VisionLibrary.Cognex;

namespace VASFx.Core
{
    public class CameraManager : IDisposable
    {
        IContainerProvider provider;
        SqlManager sql = null;
        CoreMessageEvent _coreMessage = null;

        IDictionary<eCamID, ICamera> _cameras = new Dictionary<eCamID, ICamera>();

        public IDictionary<eCamID, ICamera> Cameras { get => this._cameras; }
        public IDictionary<eExecuteZone, Dictionary<eGrabPosition, CogJob>> CogJobs = new Dictionary<eExecuteZone, Dictionary<eGrabPosition, CogJob>>();

        public CameraManager(IContainerProvider containerProvider, IEventAggregator eventAggregator, SqlManager sql)
        {
            this.provider = containerProvider;
            this.sql = sql;

            eventAggregator.GetEvent<ApplicationExitEvent>().Subscribe((o) => Dispose(), true);

            eventAggregator.GetEvent<GUIMessageEvent>().Unsubscribe(OnReceivedMessage);
            eventAggregator.GetEvent<GUIMessageEvent>().Subscribe(OnReceivedMessage, ThreadOption.BackgroundThread);

            _coreMessage = eventAggregator.GetEvent<CoreMessageEvent>();
        }

        public void Dispose()
        {
            Cameras.Values.EachExt(_ => _.DestoryCamera());
        }

        #region Event Subscribe Method
        private void OnReceivedMessage(GUIEventArgs obj)
        {
            switch (obj.MessageKind)
            {
                case eGUIMessageKind.CameraPropertyChanged:
                    this.ReqCameraPropertyChanged(obj);
                    break;
                case eGUIMessageKind.MainViewChanged:
                    if (obj.SubKind.GetType() == typeof(eMainMenu))
                        ReqMainViewChanged((eMainMenu)obj.SubKind);
                    break;
                case eGUIMessageKind.SystemStateChange:
                    break;
                default:
                    break;
            }
        }

        void ReqMainViewChanged(eMainMenu view)
        {
            //switch (view)
            //{
            //    case eMainMenu.None:
            //        break;
            //    case eMainMenu.Auto:
            //        break;
            //    case eMainMenu.Setup:
            //        break;
            //    case eMainMenu.Calibration:
            //        break;
            //    case eMainMenu.Interface:
            //        break;
            //    case eMainMenu.Edit:
            //        break;
            //    case eMainMenu.Log:
            //        break;
            //    default:
            //        break;
            //}
            if (view != eMainMenu.Auto)
            {
                this.Cameras.EachExt(cam => cam.Value.StopGrabContinuous());
            }
        }

        void ReqCameraPropertyChanged(GUIEventArgs args)
        {
            if (args.SubKind.GetType() == typeof(eCameraEventKind))
            {
                switch (args.SubKind)
                {
                    case eCameraEventKind.OneShotGrab:
                        this.ReqOneShotGrab(args.ProcessPosition.CamID);
                        break;
                    case eCameraEventKind.ContinuousGrab:
                        this.ReqContinuousGrab(args.ProcessPosition.CamID);
                        break;
                    case eCameraEventKind.StartGrabbing:
                        break;
                    case eCameraEventKind.StopContinususGrab:
                        this.ReqStopContinuousGrab(args);
                        break;
                    case eCameraEventKind.GrabComplated:
                        break;
                    default:
                        break;
                }
            }
        }

        private void ReqStopContinuousGrab(GUIEventArgs obj)
        {
            var camID = obj.ProcessPosition.CamID;
            var cam = this.Cameras[camID];
            this.StopGrabContinuous(camID);
            //cam.Stop();
        }

        void ReqOneShotGrab(eCamID camID)
        {
            var grabImage = this.GrabOneShot(camID);

            var message = new CoreEventArgs();
            message.MessageKind = eCoreMessageKind.CameraPropertyChanged;
            message.SubKind = eCameraEventKind.OneShotGrab;
            message.processPosition.CamID = camID;
            message.Arg = grabImage;
            this._coreMessage.Publish(message);
        }

        void ReqContinuousGrab(eCamID camID)
        {
            this.GrabContinuous(camID);
        }
        #endregion

        #region Camera

        public void CreateCameras()
        {
            var cll = this.sql.CameraInfo.GetAll();
            Assert.NotNull(cll, "CamaraManager Init - DB Camera Info is Null");

            cll.EachExt(info =>
            {
                var cam = new CognexCam(info);
                var tt = cam.Connect();
                cam.ImageGrabbed += Cam_ImageGrabbed;
                cam.GrabStarted += Cam_GrabStarted;
                cam.GrabStopped += Cam_GrabStopped;

                this.Cameras.Add(info.CamID, cam);
            });
        }

        private void Cam_GrabStopped(eCamID camID)
        {
            var msg = new CoreEventArgs();
            msg.MessageKind = eCoreMessageKind.CameraPropertyChanged;
            msg.SubKind = eCameraEventKind.StopGrabbing;
            msg.processPosition.CamID = camID;
            this._coreMessage.Publish(msg);
        }

        private void Cam_GrabStarted(eCamID camID)
        {
            var msg = new CoreEventArgs();
            msg.MessageKind = eCoreMessageKind.CameraPropertyChanged;
            msg.SubKind = eCameraEventKind.StartGrabbing;
            msg.processPosition.CamID = camID;
            this._coreMessage.Publish(msg);
        }

        private void Cam_ImageGrabbed(byte[] arg1, eCamID arg2)
        {
            var message = new CoreEventArgs();
            message.MessageKind = eCoreMessageKind.CameraPropertyChanged;
            message.SubKind = eCameraEventKind.GrabComplated;
            message.processPosition.CamID = arg2;

            var cam = this.Cameras[arg2];
            message.Arg = new GrabResult() { CamID = arg2, ImageHeight = cam.Height, ImageWidth = cam.Width, PixelData = arg1 };
            this._coreMessage.Publish(message);
        }

        public object GrabOneShot(eCamID camID)
        {
            var cam = this.Cameras[camID];
            return cam.GrabOneShot();
        }

        /// <summary>
        /// ImageGrabbed 이벤트 구독 필요.
        /// </summary>
        /// <param name="camID"></param>
        public void GrabContinuous(eCamID camID)
        {
            var cam = this.Cameras[camID];
            //cam.SetContinuousGrab(true);
            cam.GrabContinuous();
        }

        /// <summary>
        /// Grab Continuous Stop
        /// </summary>
        /// <param name="camID"></param>
        public void StopGrabContinuous(eCamID camID)
        {
            var cam = this.Cameras[camID] as CognexCam;
            //cam.SetContinuousGrab(false);
            cam.StopGrabContinuous();
        }

        #endregion

        #region Job

        public void CreateJobs()
        {
            var cll = this.sql.JobInfo.GetAll();
            Assert.NotNull(cll, "CamaraManager Init - DB Job Info is Null");
            var model = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;

            cll.EachExt(info =>
            {
                var Job = new Dictionary<eGrabPosition, CogJob>();
                var zone = info.ZoneID;

                for (eGrabPosition i = eGrabPosition.First; i < info.GrabPos; i++)
                {
                    var job = new CogJob(info);

                    job.JobInitialize(zone, (eGrabPosition)i, model.Name);

                    Job.Add((eGrabPosition)i, job);
                }

                this.CogJobs.Add(zone, Job);
            });
        }
        public void ModelChangeJobs(string modelName)
        {
            var cll = this.sql.JobInfo.GetAll();
            Assert.NotNull(cll, "CamaraManager Init - DB Job Info is Null");

            if (this.CogJobs.Count != 0)
                this.CogJobs.Clear();

            cll.EachExt(info =>
            {
                var Job = new Dictionary<eGrabPosition, CogJob>();
                var zone = info.ZoneID;

                for (eGrabPosition i = eGrabPosition.First; i < info.GrabPos; i++)
                {
                    var job = new CogJob(info);

                    job.JobInitialize(zone, (eGrabPosition)i, modelName);

                    Job.Add((eGrabPosition)i, job);
                }

                this.CogJobs.Add(zone, Job);
            });
        }
        #endregion
    }
}
