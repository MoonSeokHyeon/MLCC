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

namespace VASFx.UI.EditControlViews.UI
{
    public class EditSingleViewModel : BindableBase, IWindowViewModel
    {
        #region Property
        public LoaderEditViewTrayInspection _trayInspection;
        public LoaderEditViewTrayInspection TrayInspection
        {
            get { return _trayInspection; }
            set { SetProperty(ref _trayInspection, value); }
        }

        public LoaderEditViewPanelInspection _panelInspection;
        public LoaderEditViewPanelInspection PanelInspection
        {
            get { return _panelInspection; }
            set { SetProperty(ref _panelInspection, value); }
        }

        public string _camNum;
        public string CamNum
        {
            get { return _camNum; }
            set { SetProperty(ref _camNum, value); }
        }

        CogDisplaySingleView _camViewHost = null;
        public CogDisplaySingleView CamViewHost { get => this._camViewHost; set => SetProperty(ref this._camViewHost, value); }
        public IWindowView View { get; set; }
        #endregion

        #region ICommand
        public ICommand Close { get; set; }
        public ICommand Save { get; set; }
        #endregion
        public string Title => "Cam1";

        public event Action<IDialogResult> RequestClose;
        IContainerProvider provider;

        public EditSingleViewModel(IContainerProvider provider)
        {
            this.provider = provider;
            Close = new DelegateCommand(ExecuteCloseCommand);
            Save = new DelegateCommand(ExecuteSaveCommand);

            
        }

        internal void Init()
        {
            this.TrayInspection = provider.Resolve<LoaderEditViewTrayInspection>();
            this.PanelInspection = provider.Resolve<LoaderEditViewPanelInspection>();
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
