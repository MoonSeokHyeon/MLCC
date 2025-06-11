using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System.Windows.Input;
using VASFx.Common.Interface;
using VASFx.Common.Shared;
using VASFx.UI.CogDisplayViews.Views;

namespace VASFx.UI.CalibrationControlViews.UI
{
    public class CalibrationViewModel : BindableBase, IWindowViewModel
    {
        #region Properties

        CogDisplayView leftDisplay = null;
        public CogDisplayView LeftDisplay { get => this.leftDisplay; set => SetProperty(ref this.leftDisplay, value); }

        CogDisplayView rightDisplay = null;
        public CogDisplayView RightDisplay { get => this.rightDisplay; set => SetProperty(ref this.rightDisplay, value); }

        public IWindowView View { get; set; }
        public eCamID CamID { get; set; }

        #endregion

        #region ICommands

        public ICommand CloseDialogCommand { get; set; }

        #endregion

        public eExecuteZone ZoneID { get; set; }

        IContainerProvider provider = null;

        public CalibrationViewModel(IContainerProvider prov)
        {
            this.provider = prov;

            LeftDisplay = provider.Resolve<CogDisplayView>();
            RightDisplay = provider.Resolve<CogDisplayView>();

            InitICommands();
        }

        private void InitICommands()
        {
            this.CloseDialogCommand = new DelegateCommand(ExcuteCloseDialogCommand);
        }

        private void ExcuteCloseDialogCommand()
        {
            this.View.Close();
            //DialogHost.CloseDialogCommand.Execute(null, null);
        }

        public void Clear()
        {
            
        }

        public void Init()
        {
            //LeftDisplay.ViewModel.FixAirspace = false;
            //RightDisplay.ViewModel.FixAirspace = false;
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

        public void SetUserControlViewModelCamID()
        {
        }
    }
}
