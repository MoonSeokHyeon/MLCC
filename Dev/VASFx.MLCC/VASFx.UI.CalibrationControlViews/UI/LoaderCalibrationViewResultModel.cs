using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VASFx.UI.CalibrationControlViews.UI 
{
    public class LoaderCalibrationViewResultModel : BindableBase, IDialogAware
    {

        #region Properties

        public double _fovX = 150.29;
        public double FovX
        {
            get { return _fovX; }
            set { SetProperty(ref _fovX, value); }
        }

        public double _fovY = 150.29;
        public double FovY
        {
            get { return _fovY; }
            set { SetProperty(ref _fovY, value); }
        }

        public double _pixelX = 0.45;
        public double PixelX
        {
            get { return _pixelX; }
            set { SetProperty(ref _pixelX, value); }
        }

        public double _pixelY = 0.12;
        public double PixelY
        {
            get { return _pixelY; }
            set { SetProperty(ref _pixelY, value); }
        }

        public double _rmsImage = 0.5;
        public double RmsImage
        {
            get { return _rmsImage; }
            set { SetProperty(ref _rmsImage, value); }
        }

        public double _rmsHome = 0.056;
        public double RmsHome
        {
            get { return _rmsHome; }
            set { SetProperty(ref _rmsHome, value); }
        }

        public int _resultValue = 20;
        public int ResultValue
        {
            get { return _resultValue; }
            set { SetProperty(ref _resultValue, value); }
        }

        #endregion

        public ICommand Close { get; set; }
        string IDialogAware.Title => "LoaderCalibrationViewResultModel";

        public event Action<IDialogResult> RequestClose;
        IContainerProvider provider;

        public LoaderCalibrationViewResultModel(IContainerProvider provider)
        {
            this.provider = provider;
            Close = new DelegateCommand<string>(ExecuteCloseCommand);
        }

        internal void Init()
        {

        }

        private void ExecuteCloseCommand(string obj)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
