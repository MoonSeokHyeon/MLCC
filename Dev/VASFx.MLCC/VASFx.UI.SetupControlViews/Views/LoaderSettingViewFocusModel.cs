using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.Interface;
using VASFx.Core;
using VASFx.Device.Camera;

namespace VASFx.UI.SetupControlViews.Views
{
    public class LoaderSettingViewFocusModel : BindableBase
    {

        public string _resultValue = "Success";
        public string ResultValue
        {
            get { return _resultValue; }
            set { SetProperty(ref _resultValue, value); }
        }
        public ICommand SearchRegion { get; set; }
        public ICommand Set { get; set; }
        public ICommand Save { get; set; }

        ICamera _loaderCamera = null;
        ICamera _unloaderCamera = null;
        CameraManager _cameraManager = null;
        public string _checkLoader;
        public LoaderSettingViewParams _loaderParams;
        public LoaderSettingViewParams _unloaderParams;
        public LoaderSettingViewFocusModel(CameraManager cameraManager)
        {
            this._cameraManager = cameraManager;
            SearchRegion = new DelegateCommand(ExecuteSearchRegionCommand); 
            Set = new DelegateCommand(ExecuteSetCommand);
            Save = new DelegateCommand(ExecuteSaveCommand);
        }

        internal void Init()
        {
            //this._loaderCamera = _cameraManager.Cameras[Common.Shared.eCamID.Cam1];
            //this._unloaderCamera = _cameraManager.Cameras[Common.Shared.eCamID.Cam2];
        }

        private void ExecuteSaveCommand()
        {
            if (_checkLoader.Equals("Loader"))
            {
                /*_loaderCamera.SetExposureTimeAbs(_loaderParams.ViewModel.ExposureValue);
                _loaderCamera.SetGainRaw(_loaderParams.ViewModel.GainValue);
                _loaderCamera.SetGamma(_loaderParams.ViewModel.GammaValue);
                _loaderCamera.SetDigitalShift(_loaderParams.ViewModel.DigitalShiftValue);
                var exposure = _loaderCamera.GetExposureTimeAbs();
                var gain = _loaderCamera.GetGainRaw();
                var gamma = _loaderCamera.GetGamma();
                var digitalShift = _loaderCamera.GetDigitalShift();*/
            }
            else
            {
                /*_unloaderCamera.SetExposureTimeAbs(_unloaderParams.ViewModel.ExposureValue);
                _unloaderCamera.SetGainRaw(_unloaderParams.ViewModel.GainValue);
                _unloaderCamera.SetGamma(_unloaderParams.ViewModel.GammaValue);
                _unloaderCamera.SetDigitalShift(_unloaderParams.ViewModel.DigitalShiftValue);
                var exposure = _unloaderCamera.GetExposureTimeAbs();
                var gain = _unloaderCamera.GetGainRaw();
                var gamma = _unloaderCamera.GetGamma();
                var digitalShift = _unloaderCamera.GetDigitalShift();*/
            }
        }

        private void ExecuteSearchRegionCommand()
        {
            
        }

        private void ExecuteSetCommand()
        {

        }
    
    }
}
