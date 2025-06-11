using Cognex.VisionPro;
using GSG.NET.Extensions;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using VASFx.Common.Events;
using VASFx.Common.Interface;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.Core;
using VASFx.Device.Camera.Cameras;
using VASFx.MLCC.Sqlite;
using VASFx.VisionLibrary.Cognex;

namespace VASFx.UI.CogDisplayViews.Views
{
    public class CogDisplaySingleViewModel : BindableBase
    {
        #region Properties
        CogDisplayView cogDisplay = null;
        public CogDisplayView CogDisplay { get => this.cogDisplay; set => SetProperty(ref this.cogDisplay, value); }

        private bool _isLIve;

        public bool IsLive
        {
            get { return _isLIve; }
            set { SetProperty(ref this._isLIve, value); }
        }
        public eCamID camID { get; set; }
        public eExecuteZone zoneID { get; set; }
        public eGrabPosition posID { get; set; }
        public eBlobNum BlobID { get; set; }

        eSystemState systemState = eSystemState.Auto;
        IContainerProvider provider = null;
        CameraManager cameraManager = null;
        ICamera camera = null;
        CogVisionPro _lib = null;
        SqlManager _sql = null;
        IEventAggregator _eventAggregator = null;
        GUIMessageEvent _gUIMessageEvent = null;
        bool isInited = false;

        #endregion

        #region ICommands

        public ICommand GrabCommand { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand LiveCommand { get; set; }
        public ICommand CusorCommand { get; set; }
        public ICommand PanCommand { get; set; }
        public ICommand ZoomInCommand { get; set; }
        public ICommand ZoomOutCommand { get; set; }
        public ICommand FitCommand { get; set; }

        #endregion

        #region Struct
        public CogDisplaySingleViewModel(IContainerProvider prov, CameraManager cameraManager, CogVisionPro lib, SqlManager sql, IEventAggregator aggregator)
        {
            InitializeCommand();

            provider = prov;
            this.cameraManager = cameraManager;
            this._lib = lib;
            this._sql = sql;

            this._eventAggregator = aggregator;
            this._eventAggregator.GetEvent<CoreMessageEvent>().Unsubscribe(UICallbackCommunication);
            this._eventAggregator.GetEvent<CoreMessageEvent>().Subscribe(UICallbackCommunication, ThreadOption.UIThread);

            this._eventAggregator.GetEvent<WinformHostFixAirSpace>().Subscribe(SetCogDisplayAirSpace, ThreadOption.UIThread);

            this._gUIMessageEvent = this._eventAggregator.GetEvent<GUIMessageEvent>();
        }
        void InitializeCommand()
        {
            this.GrabCommand = new DelegateCommand(ExecuteGrabCommand);
            this.LiveCommand = new DelegateCommand(ExecuteLiveCommand);
            this.FindCommand = new DelegateCommand(ExecuteFindCommand);
            this.CusorCommand = new DelegateCommand(ExecuteCursorCommand);
            this.PanCommand = new DelegateCommand(ExecutePanCommand);
            this.ZoomInCommand = new DelegateCommand(ExecuteZoomInCommand);
            this.ZoomOutCommand = new DelegateCommand(ExecuteZoomOutCommand);
            this.FitCommand = new DelegateCommand(ExecuteFitCommand);
        }
        public void Init()
        {
            if (isInited) return;
            isInited = true;

            this.IsLive = false;

            zoneID = eExecuteZone.MLCC_INSPECTION;
            posID = eGrabPosition.First;
            camID = eCamID.Cam1;

            CogDisplay = provider.Resolve<CogDisplayView>();
            CogDisplay.CogDisplayLoaded += CogDisplay_Loaded;

            this.camera = cameraManager.Cameras.Values.FirstOrDefault(_ => _.CamID == this.camID);
            var cogCam = this.camera as CognexCam;
            cogCam.CogImageGrabbed += CogCam_CogImageGrabbed;
        }
        private void SetCogDisplayAirSpace(bool obj)
        {
            this.CogDisplay.FixAirspace = obj;
        }

        #endregion

        #region UI Call Back Communication
        private void UICallbackCommunication(CoreEventArgs obj)
        {
            if (this.camID != obj.processPosition.CamID) return;

            switch (obj.MessageKind)
            {
                case eCoreMessageKind.SystemStateChange:
                    break;
                case eCoreMessageKind.ModelChange:
                    break;
                case eCoreMessageKind.ChangeModelList:
                    break;
                case eCoreMessageKind.LightValueChange:
                    break;
                case eCoreMessageKind.Calibration:
                    break;
                case eCoreMessageKind.CalibrationComplete:
                    break;
                case eCoreMessageKind.AddedAlignHistory:
                    break;
                case eCoreMessageKind.AddedInspectionHistory:
                    break;
                case eCoreMessageKind.FailModelChange:
                    break;
                case eCoreMessageKind.CameraPropertyChanged:
                    switch (obj.SubKind)
                    {
                        case eCameraEventKind.OneShotGrab:
                            CallBackOneShotGrab(obj);
                            break;
                        default:
                            break;
                    }
                    break;
                case eCoreMessageKind.ResultChange:
                    OnReceiveGraphics(obj);
                    break;
                default:
                    break;
            }
        }

        private void CallBackOneShotGrab(CoreEventArgs obj)
        {
            var image = CastTo<ICogImage>.From(obj.Arg);
            if (image != null)
                this.CogDisplay.SetImage(image);
        }
        #endregion

        #region Private Method

        private void CogDisplay_Loaded()
        {
            var graphic = this._lib.GetGraphicCollectionDrawCenterGrid(this.camera.Width, camera.Height);
            CogDisplay.SetGraphic(graphic, "CenterGrad", true);
        }

        private void CogCam_CogImageGrabbed(Cognex.VisionPro.ICogImage obj)
        {
            this.CogDisplay.SetImage(obj);
        }
        private void OnReceiveGraphics(CoreEventArgs obj)
        {
            this.CogDisplay.ClearImage();
            this.CogDisplay.ClearGraphics();

            var msg = obj.Arg as ResultMLCCInspection;

            var image = msg.Image as ICogImage;
            var graphics = msg.Graphics as CogGraphicInteractiveCollection;

            if (image == null)
                return;

            this.CogDisplay.SetImage(image);

            _lib.DrawSixGrid(image, graphics);

            CogDisplay.SetGraphic(graphics, "BlobResult", true);
        }

        #endregion

        #region Command

        private void ExecuteGrabCommand()
        {
            var centerGrid = new CogGraphicInteractiveCollection();

            if (this.cogDisplay.IsLiveDisplay())
            {
                this.cogDisplay.StopLiveDisplay();
                this.IsLive = false;
            }

            this.CogDisplay.ClearImage();
            this.CogDisplay.ClearGraphics();

            var image = (ICogImage)this.camera.GrabOneShot();

            this.CogDisplay.SetImage(image);

            _lib.DrawCenterGrid(image, centerGrid);
            this.CogDisplay.SetGraphic(centerGrid, "CenterGrid", true);

        }
        private void ExecuteLiveCommand()
        {
            var centerGrid = new CogGraphicInteractiveCollection();
            if (this.systemState == eSystemState.Auto) return;

            if (this.cogDisplay.IsLiveDisplay())
            {
                this.cogDisplay.StopLiveDisplay();
                this.IsLive = false;
                return;
            }
            var FIFO = this.camera as CognexCam;

            this.IsLive = true;

            this.CogDisplay.StartLiveDisplay(FIFO.Fifo);
            this.CogDisplay.SetGraphic(centerGrid, "CenterGrid", true);
        }

        private void ExecuteFindCommand()
        {
            var message = new GUIEventArgs();
            message.MessageKind = eGUIMessageKind.RunProcesser;
            message.ProcessPosition.ZoneID = zoneID;
            message.ProcessPosition.GrabPosition = posID;
            message.ProcessPosition.CamID = camID;
            message.LoadImage = null;
            this._gUIMessageEvent.Publish(message);
        }

        private void ExecuteCursorCommand()
        {
            CogDisplay.SetMouseMode(eDisplayMouseMode.Pointer);
        }

        private void ExecutePanCommand()
        {
            CogDisplay.SetMouseMode(eDisplayMouseMode.Pan);
        }

        private void ExecuteZoomInCommand()
        {
            CogDisplay.SetMouseMode(eDisplayMouseMode.ZoomIn);
        }

        private void ExecuteZoomOutCommand()
        {
            CogDisplay.SetMouseMode(eDisplayMouseMode.ZoomOut);
        }

        private void ExecuteFitCommand()
        {
            CogDisplay.SetFitImage();
        }

        #endregion
    }
}
