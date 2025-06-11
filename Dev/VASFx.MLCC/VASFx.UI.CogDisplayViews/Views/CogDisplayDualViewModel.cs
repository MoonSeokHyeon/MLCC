using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Windows.Input;
using VASFx.Common.Shared;

namespace VASFx.UI.CogDisplayViews.Views
{
    public class CogDisplayDualViewModel : BindableBase
    {
        public CogDisplayView CogDisplay1 { get => this.cogDisplay1; set => SetProperty(ref this.cogDisplay1, value); }
        public CogDisplayView CogDisplay2 { get => this.cogDisplay2; set => SetProperty(ref this.cogDisplay2, value); }


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

        public eCamID display1CamID { get; set; }
        public eCamID display2CamID { get; set; }

        CogDisplayView cogDisplay1 = null;

        CogDisplayView cogDisplay2 = null;

        IContainerProvider provider = null;

        public CogDisplayDualViewModel(IContainerProvider prov)
        {
            provider = prov;

            CogDisplay1 = provider.Resolve<CogDisplayView>();
            CogDisplay2 = provider.Resolve<CogDisplayView>();

            InitializeCommand();
        }

        bool isInited = false;
        public void Init()
        {
            if (isInited) return;
            isInited = true;

            //baslerCamera1 = visionManager.cameraManager.CameraDic[display1CamID];
            //baslerCamera2 = visionManager.cameraManager.CameraDic[display2CamID];

            //baslerCamera1.ImageGrabbed += BaslerCamera_ImageGrabbed;
            //baslerCamera2.ImageGrabbed += BaslerCamera_ImageGrabbed;
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
            //CogDisplay1.ViewModel.SetBitmapImage(baslerCamera1.GrabOneShot());
            //CogDisplay2.ViewModel.SetBitmapImage(baslerCamera2.GrabOneShot());
        }

        private void ExecuteLiveCommand()
        {
            //baslerCamera1.GrabContinuous();
            //baslerCamera2.GrabContinuous();
        }

        private void ExecuteFindCommand()
        {

        }

        private void ExecuteCursorCommand()
        {
            //CogDisplay1.ViewModel.SetMouseMode(eDisplayMouseMode.Pointer);
            //CogDisplay2.ViewModel.SetMouseMode(eDisplayMouseMode.Pointer);
        }

        private void ExecutePanCommand()
        {
            //CogDisplay1.ViewModel.SetMouseMode(eDisplayMouseMode.Pan);
            //CogDisplay2.ViewModel.SetMouseMode(eDisplayMouseMode.Pan);
        }

        private void ExecuteZoomInCommand()
        {
            //CogDisplay1.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomIn);
            //CogDisplay2.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomIn);
        }

        private void ExecuteZoomOutCommand()
        {
            //CogDisplay1.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomOut);
            //CogDisplay2.ViewModel.SetMouseMode(eDisplayMouseMode.ZoomOut);
        }

        private void ExecuteFitCommand()
        {
            //CogDisplay1.ViewModel.FitImage();
            //CogDisplay2.ViewModel.FitImage();
        }

        private void ExecuteRulerSquareCommand()
        {

        }
    }
}
