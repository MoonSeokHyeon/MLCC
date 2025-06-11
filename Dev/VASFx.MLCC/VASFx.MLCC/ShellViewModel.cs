using GSG.NET.Extensions;
using GSG.NET.Logging;
using GSG.NET.PLC.Mitsubishi;
using GSG.NET.PLC.SLMP;
using GSG.NET.PLC.Support;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using VASFx.Common.Events;
using VASFx.Common.Shared;
using VASFx.MLCC.Sqlite;
using VASFx.MLCC.UI.EditViews;
using VASFx.MLCC.UI.ImageLogViews;
using VASFx.MLCC.UI.InterfaceViews;
using VASFx.MLCC.UI.LogViews;
using VASFx.MLCC.UI.MainViews;
using VASFx.MLCC.UI.OptionViews;
using VASFx.MLCC.UI.SettingViews;
using VASFx.UI.Interactivity;

namespace VASFx.MLCC
{
    public class ShellViewModel : BindableBase, IDisposable
    {
        Logger logger = Logger.GetLogger();

        #region Properties

        private double _cpu;
        public double CPU
        {
            get { return this._cpu; }
            set { this.SetProperty(ref this._cpu, value); }
        }

        private double _totalCPU;
        public double TotalCPU
        {
            get { return this._totalCPU; }
            set { this.SetProperty(ref this._totalCPU, value); }
        }

        private double _ram;
        public double RAM
        {
            get { return this._ram; }
            set { this.SetProperty(ref this._ram, value); }
        }

        listBoxTemplate cpuUsage;
        public listBoxTemplate CPUUsage
        {
            get { return this.cpuUsage; }
            set { SetProperty(ref this.cpuUsage, value); }
        }

        listBoxTemplate ramUsage;
        public listBoxTemplate RAMUsage
        {
            get { return this.ramUsage; }
            set { SetProperty(ref this.ramUsage, value); }
        }

        private List<listBoxTemplate> hddDriveList;

        public List<listBoxTemplate> HDDDriveList
        {
            get { return this.hddDriveList; }
            set { SetProperty(ref this.hddDriveList, value); }
        }

        private bool isPLCConnected;
        public bool IsPLCConnected
        {
            get { return isPLCConnected; }
            set { this.SetProperty(ref this.isPLCConnected, value); }
        }
        private String statePLC;
        public String StatePLC
        {
            get { return statePLC; }
            set { this.SetProperty(ref this.statePLC, value); }
        }
        private long plcScanTime;
        public long PLCScanTime
        {
            get { return plcScanTime; }
            set { SetProperty(ref plcScanTime, value); }
        }

        DateTime _dateTime;
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { this.SetProperty(ref _dateTime, value); }
        }

        private string currentModelName;

        public string CurrentModelName
        {
            get { return currentModelName; }
            set { SetProperty(ref this.currentModelName, value); }
        }

        private List<string> modelList;

        public List<string> ModelList
        {
            get { return this.modelList; }
            set { SetProperty(ref this.modelList, value); }
        }

        private string systemID;
        public string SystemID
        {
            get { return systemID; }
            set { SetProperty(ref this.systemID, value); }
        }

        bool _testChecked = false;
        public bool TestChecked
        {
            get => this._testChecked;
            set
            {
                if (SetProperty(ref this._testChecked, value))
                {
                    this.winformHostEventPublishrer.Publish(value);
                }
            }
        }
        eSystemState systemState = eSystemState.None;
        public eSystemState SystemState { get => this.systemState; set => SetProperty(ref this.systemState, value); }

        private eMainMenu _selectedMainManu = eMainMenu.None;

        public eMainMenu SelectedMainManu
        {
            get { return _selectedMainManu = eMainMenu.None; }
            set { SetProperty(ref _selectedMainManu, value); }
        }

        #endregion

        #region Command
        public ICommand SelectedMainManuCommand { get; set; }

        public ICommand OptionViewCommand { get; set; }

        public ICommand SystemOffCommand { get; set; }

        public ICommand WindowMinCommand { get; set; }

        #endregion

        public Shell View { get; set; }

        SlmpManager plc = null;
        IContainerProvider provider = null;
        IRegionManager regionManager = null;
        SqlManager sql = null;
        IEventAggregator eventAggregator = null;
        WinformHostFixAirSpace winformHostEventPublishrer = null;

        public ShellViewModel(IEventAggregator aggregator, IRegionManager region, IContainerProvider provider, SlmpManager miz, SqlManager sqlManager)
        {
            this.eventAggregator = aggregator;
            this.regionManager = region;
            this.sql = sqlManager;
            this.plc = miz;
            this.provider = provider;

            winformHostEventPublishrer = aggregator.GetEvent<WinformHostFixAirSpace>();

            aggregator.GetEvent<GUIMessageEvent>().Subscribe(UICallBackCommunication, ThreadOption.UIThread);
            aggregator.GetEvent<ApplicationExitEvent>().Subscribe((o) => Dispose(), true);


            DispatcherTimer dateTimer = new DispatcherTimer();
            dateTimer.Tick += (object sender, EventArgs e) =>
            {
                this.DateTime = DateTime.Now;
            };
            dateTimer.Interval = TimeSpan.FromMilliseconds(500);
            dateTimer.Start();

            InitDelegeteCommand();

            GSG.NET.Quartz.QuartzUtils.Invoke("RESOURCE_CHECK", GSG.NET.Quartz.QuartzUtils.GetExpnSecond(3), QuzOnResourceUsage);
            GSG.NET.Quartz.QuartzUtils.Invoke("PLC_CHECK", GSG.NET.Quartz.QuartzUtils.GetExpnSecond(3), ConnectedCheckPLC);

            this.CPUUsage = new listBoxTemplate() { Tag = "CPU" };
            this.RAMUsage = new listBoxTemplate() { Tag = "RAM" };

            this.HDDDriveList = new List<listBoxTemplate>();
            var ll = GSG.NET.OSView.Mgnt.HddList();
            ll.ForEach(x =>
            {
                var hdd = new listBoxTemplate();
                hdd.Tag = x.Name;
                var free = x.AvailableFreeSpace;
                var total = x.TotalSize;
                var usage = x.TotalSize - x.AvailableFreeSpace;
                hdd.Value1 = (usage / total) * 100;

                this.HDDDriveList.Add(hdd);
            });
        }
        public void Init()
        {
            SystemState = eSystemState.Auto;

            var systeminfo = sql.SystemInfo.GetAll().FirstOrDefault();
            this.SystemID = systeminfo.SystemID;

            plc.OnCollected += Plc_OnCollected;
            this.plc.OnConnect += Plc_OnConnect;
            this.plc.OnDisconnect += Plc_OnDisconnect;

            this.SelectedMainManu = eMainMenu.Auto;
            this.regionManager.RequestNavigate(RegionNames.MainView, nameof(MLCCMainView));
        }

        #region UI CallBack Communication
        private void UICallBackCommunication(GUIEventArgs obj)
        {
            switch (obj.MessageKind)
            {
                case eGUIMessageKind.SystemStateChange:
                    ReqSystemStateChange(obj);
                    break;
                case eGUIMessageKind.ModelChange:
                    //ReqModelChange();
                    break;
                case eGUIMessageKind.CalibrationComplete:
                    break;
                case eGUIMessageKind.AddedAlignHistory:
                    break;
                case eGUIMessageKind.AddedInspectionHistory:
                    break;
                default:
                    break;
            }
        }
        private void ReqSystemStateChange(GUIEventArgs obj)
        {
            this.SystemState = CastTo<eSystemState>.From<object>(obj.Arg);
        }
        #endregion

        #region PLC Event
        private void Plc_OnDisconnect(string id)
        {
            this.IsPLCConnected = false;
            this.StatePLC = "PLC Disconnected";
        }

        private void Plc_OnConnect(string id)
        {
            this.IsPLCConnected = true;
            this.StatePLC = "PLC Connected";
        }

        int collectedCount = 0; 
        private void Plc_OnCollected(MapScan scan)
        {
            collectedCount++;
            if (collectedCount == 50)
            {
                PLCScanTime = scan.CollectTime;
                collectedCount = 0;
            }
        }
        #endregion

        void InitDelegeteCommand()
        {
            this.SelectedMainManuCommand = new DelegateCommand<string>(ExecuteSelectedMainManuCommand);
            this.OptionViewCommand = new DelegateCommand(ExecuteOptionViewCommand);
            this.SystemOffCommand = new DelegateCommand(ExecuteSystemOff);
            this.WindowMinCommand = new DelegateCommand(ExecuteWindowMinCommand);
        }

        private void ExecuteSelectedMainManuCommand(string obj)
        {
            switch (obj)
            {
                case "Main":
                    this.regionManager.RequestNavigate(RegionNames.MainView, nameof(MLCCMainView));
                    break;

                case "Setup":
                    //this.regionManager.RequestNavigate(RegionNames.MainView, nameof(MLCCSettingView));
                    break;

                case "Calibration":
                    //this.regionManager.RequestNavigate(RegionNames.MainView, nameof(ChamberCalibrationView));
                    break;

                case "Edit":
                    this.regionManager.RequestNavigate(RegionNames.MainView, nameof(MLCCEditView));
                    break;

                case "Interface":
                    this.regionManager.RequestNavigate(RegionNames.MainView, nameof(MLCCInterfaceView));
                    break;

                case "DataLog":
                    this.regionManager.RequestNavigate(RegionNames.MainView, nameof(MLCCLogView));
                    break;

                case "ImageLog":
                    this.regionManager.RequestNavigate(RegionNames.MainView, nameof(MLCCMainImageLogView));
                    break;

                default:
                    break;
            }

        }

        private async void ExecuteOptionViewCommand()
        {
            var view = this.provider.Resolve<MLCCOptionView>();
            await DialogHost.Show(view, "RootDialog");
        }

        private void ExecuteWindowMinCommand()
        {
            View.WindowState = System.Windows.WindowState.Minimized;
        }

        private async void ExecuteSystemOff()
        {
            var view = this.provider.Resolve<ComfirmationView>();
            view.ViewModel.Message = "System Off ?";

            var result = await DialogHost.Show(view, "RootDialog") as bool?;
            if (result == true)
                App.Current.Shutdown();
        }

        void QuzOnResourceUsage()
        {
            try
            {
                this.CPU = GSG.NET.OSView.Mgnt.CpuUseRate();
                this.CPUUsage.Value1 = this.CPU;

                this.RAM = Math.Abs(GSG.NET.OSView.Mgnt.MemPhysicalUseRate());
                this.RAMUsage.Value1 = this.RAM;

                var ll = GSG.NET.OSView.Mgnt.HddList();
                ll.ForEach(hdd =>
                {
                    var hddUsage = this.HDDDriveList.FirstOrDefault(x => x.Tag.Equals(hdd.Name));
                    var free = hdd.AvailableFreeSpace;
                    double total = hdd.TotalSize;
                    double usage = hdd.TotalSize - hdd.AvailableFreeSpace;
                    hddUsage.Value1 = (usage / total) * 100;
                });

            }
            catch (Exception ex)
            {
                logger.E(ex);
            }
        }
        void ConnectedCheckPLC()
        {
            try
            { 
                string id = "";

                if (this.plc.IsConnected) 
                    Plc_OnConnect(id);
                else 
                    Plc_OnDisconnect(id);
            }
            catch (Exception ex)
            {
                logger.E(ex);
            }
        }

        public void Dispose()
        {
            GSG.NET.Quartz.QuartzUtils.StopSchedule("RESOURCE_CHECK");
            GSG.NET.Quartz.QuartzUtils.StopSchedule("PLC_CHECK");
        }

        public class listBoxTemplate : BindableBase
        {
            private string tag;
            public string Tag
            {
                get { return tag; ; }
                set { SetProperty(ref this.tag, value); }
            }

            private double value1;

            public double Value1
            {
                get { return value1; }
                set { SetProperty(ref this.value1, value); }
            }
        }
    }
}
