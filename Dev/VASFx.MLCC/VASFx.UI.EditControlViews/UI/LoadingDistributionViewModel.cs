using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.UI.LogControls.ChartViews;

namespace VASFx.UI.EditControlViews.UI
{
    public class LoadingDistributionViewModel : BindableBase
    {
        #region Properties

        LoadingdistributionView unit1GlassLoadingControl = null;
        public LoadingdistributionView Unit1GlassLoadingControl { get => this.unit1GlassLoadingControl; set => SetProperty(ref this.unit1GlassLoadingControl, value); }

        LoadingdistributionView unit1PanelLoadingControl = null;
        public LoadingdistributionView Unit1PanelLoadingControl { get => this.unit1PanelLoadingControl; set => SetProperty(ref this.unit1PanelLoadingControl, value); }

        LoadingdistributionView unit2GlassLoadingControl = null;
        public LoadingdistributionView Unit2GlassLoadingControl { get => this.unit2GlassLoadingControl; set => SetProperty(ref this.unit2GlassLoadingControl, value); }

        LoadingdistributionView unit2PanelLoadingControl = null;
        public LoadingdistributionView Unit2PanelLoadingControl { get => this.unit2PanelLoadingControl; set => SetProperty(ref this.unit2PanelLoadingControl, value); }

        private ObservableCollection<DistributionData> unit1GlassDailyReportList = new ObservableCollection<DistributionData>();

        public ObservableCollection<DistributionData> Unit1GlassDailyReportList
        {
            get { return unit1GlassDailyReportList; }
            set { SetProperty(ref this.unit1GlassDailyReportList, value); ; }
        }

        private ObservableCollection<DistributionData> unit1PanelDailyReportList = new ObservableCollection<DistributionData>();

        public ObservableCollection<DistributionData> Unit1PanelDailyReportList
        {
            get { return unit1PanelDailyReportList; }
            set { SetProperty(ref this.unit1PanelDailyReportList, value); ; }
        }

        private ObservableCollection<DistributionData> unit2GlassDailyReportList = new ObservableCollection<DistributionData>();

        public ObservableCollection<DistributionData> Unit2GlassDailyReportList
        {
            get { return unit2GlassDailyReportList; }
            set { SetProperty(ref this.unit2GlassDailyReportList, value); ; }
        }

        private ObservableCollection<DistributionData> unit2PanelDailyReportList = new ObservableCollection<DistributionData>();

        public ObservableCollection<DistributionData> Unit2PanelDailyReportList
        {
            get { return unit2PanelDailyReportList; }
            set { SetProperty(ref this.unit2PanelDailyReportList, value); ; }
        }


        private ObservableCollection<DistributionData> unit1GlassHourlyReportList = new ObservableCollection<DistributionData>();

        public ObservableCollection<DistributionData> Unit1GlassHourlyReportList
        {
            get { return unit1GlassHourlyReportList; }
            set { SetProperty(ref this.unit1GlassHourlyReportList, value); ; }
        }

        private ObservableCollection<DistributionData> unit1PanelHourlyReportList = new ObservableCollection<DistributionData>();

        public ObservableCollection<DistributionData> Unit1PanelHourlyReportList
        {
            get { return unit1PanelHourlyReportList; }
            set { SetProperty(ref this.unit1PanelHourlyReportList, value); ; }
        }

        private ObservableCollection<DistributionData> unit2GlassHourlyReportList = new ObservableCollection<DistributionData>();

        public ObservableCollection<DistributionData> Unit2GlassHourlyReportList
        {
            get { return unit2GlassHourlyReportList; }
            set { SetProperty(ref this.unit2GlassHourlyReportList, value); ; }
        }

        private ObservableCollection<DistributionData> unit2PanelHourlyReportList = new ObservableCollection<DistributionData>();

        public ObservableCollection<DistributionData> Unit2PanelHourlyReportList
        {
            get { return unit2PanelHourlyReportList; }
            set { SetProperty(ref this.unit2PanelHourlyReportList, value); ; }
        }

        #endregion

        #region ICommands

        public ICommand CloseDialogCommand { get; set; }

        public ICommand Unit1GlassDailyCommand { get; set; }
        public ICommand Unit1PanelDailyCommand { get; set; }
        public ICommand Unit2GlassDailyCommand { get; set; }
        public ICommand Unit2PanelDailyCommand { get; set; }

        public ICommand Unit1GlassHourlyCommand { get; set; }
        public ICommand Unit1PanelHourlyCommand { get; set; }
        public ICommand Unit2GlassHourlyCommand { get; set; }
        public ICommand Unit2PanelHourlyCommand { get; set; }

        #endregion
        public eExecuteZone ZoneID { get; set; }

        IContainerProvider provider = null;

        public LoadingDistributionViewModel(IContainerProvider prov)
        {
            this.provider = prov;

            this.Unit1GlassLoadingControl = provider.Resolve<LoadingdistributionView>();
            this.Unit1GlassLoadingControl.ViewModel.ZoneID = Common.Shared.eExecuteZone.CHAMBER1_TARGET;

            this.Unit1PanelLoadingControl = provider.Resolve<LoadingdistributionView>();
            this.Unit1PanelLoadingControl.ViewModel.ZoneID = Common.Shared.eExecuteZone.CHAMBER1_SUBJECT;

            this.Unit2GlassLoadingControl = provider.Resolve<LoadingdistributionView>();
            this.Unit2GlassLoadingControl.ViewModel.ZoneID = Common.Shared.eExecuteZone.CHAMBER2_TARGET;

            this.Unit2PanelLoadingControl = provider.Resolve<LoadingdistributionView>();
            this.Unit2PanelLoadingControl.ViewModel.ZoneID = Common.Shared.eExecuteZone.CHAMBER2_SUBJECT;


            InitICommands();
        }

        private void InitICommands()
        {
            this.CloseDialogCommand = new DelegateCommand(ExcuteCloseDialogCommand);

            this.Unit1GlassDailyCommand = new DelegateCommand<object>(ExcuteChangeDailyReportCommand);
            this.Unit1PanelDailyCommand = new DelegateCommand<object>(ExcuteChangeDailyReportCommand);
            this.Unit2GlassDailyCommand = new DelegateCommand<object>(ExcuteChangeDailyReportCommand);
            this.Unit2PanelDailyCommand = new DelegateCommand<object>(ExcuteChangeDailyReportCommand);

            this.Unit1GlassHourlyCommand = new DelegateCommand<object>(ExcuteChangeDailyReportCommand);
            this.Unit1PanelHourlyCommand = new DelegateCommand<object>(ExcuteChangeDailyReportCommand);
            this.Unit2GlassHourlyCommand = new DelegateCommand<object>(ExcuteChangeDailyReportCommand);
            this.Unit2PanelHourlyCommand = new DelegateCommand<object>(ExcuteChangeDailyReportCommand);
        }

        private void ExcuteChangeDailyReportCommand(object obj)
        {
        }

        private void ExcuteCloseDialogCommand()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        public void Init()
        {
        }
    }
}
