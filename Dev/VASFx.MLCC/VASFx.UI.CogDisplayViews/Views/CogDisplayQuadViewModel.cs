using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Windows.Input;
using VASFx.Common.Shared;
using VASFx.Core;

namespace VASFx.UI.CogDisplayViews.Views
{
    public class CogDisplayQuadViewModel : BindableBase
    {
        #region Properties

        CogDisplayView cogDisplay1 = null;
        public CogDisplayView CogDisplay1 { get => this.cogDisplay1; set => SetProperty(ref this.cogDisplay1, value); }

        CogDisplayView cogDisplay2 = null;
        public CogDisplayView CogDisplay2 { get => this.cogDisplay2; set => SetProperty(ref this.cogDisplay2, value); }

        CogDisplayView cogDisplay3 = null;
        public CogDisplayView CogDisplay3 { get => this.cogDisplay3; set => SetProperty(ref this.cogDisplay3, value); }

        CogDisplayView cogDisplay4 = null;
        public CogDisplayView CogDisplay4 { get => this.cogDisplay4; set => SetProperty(ref this.cogDisplay4, value); }
        
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
        public ICommand RulerSquareCommand { get; set; }

        #endregion

        public eExecuteZone zoneID { get; set; }
        public eCamID display1CamID { get; set; }
        public eCamID display2CamID { get; set; }

        IContainerProvider provider = null;
        bool _isInit;

        public CogDisplayQuadViewModel(IContainerProvider prov)
        {
            this.provider = prov;

            CogDisplay1 = provider.Resolve<CogDisplayView>();
            CogDisplay2 = provider.Resolve<CogDisplayView>();
            CogDisplay3 = provider.Resolve<CogDisplayView>();
            CogDisplay4 = provider.Resolve<CogDisplayView>();

            InitializeCommand();
        }

        public void Init()
        {
            if(!_isInit)
            {
                _isInit = true;

                //CogDisplay1.ViewModel.FixAirspace = false;
                //CogDisplay2.ViewModel.FixAirspace = false;
                //CogDisplay3.ViewModel.FixAirspace = false;
                //CogDisplay4.ViewModel.FixAirspace = false;
            }

            //baslerCamera1 = visionManager.cameraManager.CameraDic[display1CamID];
            //baslerCamera2 = visionManager.cameraManager.CameraDic[display2CamID];
            //baslerCamera3 = visionManager.cameraManager.CameraDic[display3CamID];
            //baslerCamera4 = visionManager.cameraManager.CameraDic[display4CamID];

            //baslerCamera1.ImageGrabbed += BaslerCamera_ImageGrabbed;
            //baslerCamera2.ImageGrabbed += BaslerCamera_ImageGrabbed;
            //baslerCamera3.ImageGrabbed += BaslerCamera_ImageGrabbed;
            //baslerCamera4.ImageGrabbed += BaslerCamera_ImageGrabbed;
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
            this.RulerSquareCommand = new DelegateCommand(ExecuteRulerSquareCommand);
        }

        private void BaslerCamera_ImageGrabbed(System.Drawing.Bitmap arg1, eCamID id)
        {

        }

        private void ExecuteGrabCommand()
        {
            //var cam1 = this.cognexManager.CogCameras[eCamID.Cam1];
            //var cam2 = this.cognexManager.CogCameras[eCamID.Cam2];
            //var cam3 = this.cognexManager.CogCameras[eCamID.Cam3];
            //var cam4 = this.cognexManager.CogCameras[eCamID.Cam4];

            //if (cam1 == null || cam2 == null) return;

            //if(CogDisplay1.ViewModel.IsLiveDisplay() || CogDisplay2.ViewModel.IsLiveDisplay() || CogDisplay3.ViewModel.IsLiveDisplay() || CogDisplay4.ViewModel.IsLiveDisplay())
            //{
            //    CogDisplay1.ViewModel.StopLiveDisplay();
            //    CogDisplay2.ViewModel.StopLiveDisplay();
            //    CogDisplay3.ViewModel.StopLiveDisplay();
            //    CogDisplay4.ViewModel.StopLiveDisplay();
            //}

            //var grabImage1 = cam1.GrabImageCamera();
            //var grabImage2 = cam2.GrabImageCamera();
            //var grabImage3 = cam3.GrabImageCamera();
            //var grabImage4 = cam4.GrabImageCamera();
            //CogDisplay1.ViewModel.SetCogImage(grabImage1);
            //CogDisplay2.ViewModel.SetCogImage(grabImage2);
            //CogDisplay3.ViewModel.SetCogImage(grabImage3);
            //CogDisplay4.ViewModel.SetCogImage(grabImage4);
        }

        private void ExecuteLiveCommand()
        {
            //CogDisplay1.ViewModel.StartLiveDisplay(cam1);
            ////CogDisplay2.ViewModel.StartLiveDisplay(cam2);
            //CogDisplay3.ViewModel.StartLiveDisplay(cam3);
            ////CogDisplay4.ViewModel.StartLiveDisplay(cam4);
        }

        private void ExecuteFindCommand()
        {

        }

        private void ExecuteCursorCommand()
        {
            //CogDisplay1.ViewModel.SetMouseMode(eDisplayMouseMode.Pointer);
            //CogDisplay2.ViewModel.SetMouseMode(eDisplayMouseMode.Pointer);
            //CogDisplay3.ViewModel.SetMouseMode(eDisplayMouseMode.Pointer);
            //CogDisplay4.ViewModel.SetMouseMode(eDisplayMouseMode.Pointer);
        }

        private void ExecutePanCommand()
        {
            //CogDisplay1.ViewModel.SetMouseMode(eDisplayMouseMode.Pan);
            //CogDisplay2.ViewModel.SetMouseMode(eDisplayMouseMode.Pan);
            //CogDisplay3.ViewModel.SetMouseMode(eDisplayMouseMode.Pan);
            //CogDisplay4.ViewModel.SetMouseMode(eDisplayMouseMode.Pan);
        }

        private void ExecuteZoomInCommand()
        {
            //CogDisplay1.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomIn);
            //CogDisplay2.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomIn);
            //CogDisplay3.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomIn);
            //CogDisplay4.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomIn);
        }

        private void ExecuteZoomOutCommand()
        {
            //CogDisplay1.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomOut);
            //CogDisplay2.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomOut);
            //CogDisplay3.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomOut);
            //CogDisplay4.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomOut);
        }

        private void ExecuteFitCommand()
        {
            //CogDisplay1.ViewModel.FitImage();
            //CogDisplay2.ViewModel.FitImage();
            //CogDisplay3.ViewModel.FitImage();
            //CogDisplay4.ViewModel.FitImage();
        }

        private void ExecuteRulerSquareCommand()
        {

        }
    }
}
