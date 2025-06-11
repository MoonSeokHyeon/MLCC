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
using VASFx.Common.Interface;
using VASFx.Common.Shared;
using VASFx.UI.CogDisplayViews.Views;

namespace VASFx.UI.CalibrationControlViews.UI
{
    public class CalibrationSingleViewModel : BindableBase, IWindowViewModel
    {
        #region Properties
        public CogDisplaySingleView _camViewHost;
        public CogDisplaySingleView CamViewHost
        {
            get { return _camViewHost; }
            set { SetProperty(ref _camViewHost, value); }
        }

        public LoaderCalibrationViewIntrinsic _intrinsicView;
        public LoaderCalibrationViewIntrinsic IntrinsicView
        {
            get { return _intrinsicView; }
            set { SetProperty(ref _intrinsicView, value); }
        }

        public LoaderCalibrationViewAuto _autoView;
        public LoaderCalibrationViewAuto AutoView
        {
            get { return _autoView; }
            set { SetProperty(ref _autoView, value); }
        }

        public string _camNum;
        public string CamNum
        {
            get { return _camNum; }
            set { SetProperty(ref _camNum, value); }
        }


        #endregion

        #region ICommand
        public ICommand Close { get; set; }
        public ICommand Save { get; set; }
        public ICommand CalReady { get; set; }
        public ICommand CalStop { get; set; }
        #endregion

        public eExecuteZone Zone { get; set; }

        public event Action<IDialogResult> RequestClose;
        public IContainerProvider provider;

        public string Title => "Cam1";

        public IWindowView View { get; set; }

        public CalibrationSingleViewModel(IContainerProvider provider)
        {
            this.provider = provider;
            Close = new DelegateCommand(ExecuteCloseCommand);
            Save = new DelegateCommand(ExecuteSaveCommand);
            CalReady = new DelegateCommand(ExecuteCalReadyCommand);
            CalStop = new DelegateCommand(ExecuteCalStopCommand);
          


        }

        internal void Init()
        {
            IntrinsicView = provider.Resolve<LoaderCalibrationViewIntrinsic>();
            AutoView = provider.Resolve<LoaderCalibrationViewAuto>();
            this.CamViewHost = provider.Resolve<CogDisplaySingleView>();
            if (CamNum.Equals("Cam1"))
                CamViewHost.ViewModel.camID = eCamID.Cam1;
           else
                CamViewHost.ViewModel.camID = eCamID.Cam2;


        }

        private void ExecuteCloseCommand()
        {
            //DialogHost.CloseDialogCommand.Execute(null, null);
            if (!CamViewHost.ViewModel.IsLive)
            {
                //CamViewHost.ViewModel.CogDisplay.ViewModel.StopGrabContinuous();
            }

            this.View.Close();
        }

        private void ExecuteSaveCommand()
        {

        }
        private void ExecuteCalReadyCommand()
        {

        }

        private void ExecuteCalStopCommand()
        {

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

        void IWindowViewModel.Init()
        {
            
        }

        public void Query()
        {
            
        }

        public void Subscribe()
        {
            
        }

        public void Unsubscribe()
        {
            
        }

        public void Clear()
        {
            
        }
    }
}