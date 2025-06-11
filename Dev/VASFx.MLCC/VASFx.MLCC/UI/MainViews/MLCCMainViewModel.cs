using GSG.NET.Concurrent;
using GSG.NET.Extensions;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using VASFx.Common.Events;
using VASFx.Common.HistoryModel;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.MLCC.Sqlite;
using VASFx.UI.CogDisplayViews.Views;
using VASFx.UI.Interactivity;
using VASFx.UI.InterfaceView;
using VASFx.UI.LogControls.ChartViews;
using VASFx.UI.LogControls.LogViews;

namespace VASFx.MLCC.UI.MainViews
{
    public class MLCCMainViewModel : BindableBase
    {
        #region Properties
        private CogDisplaySingleView _cam1SingleView;

        public CogDisplaySingleView Cam1SingleView
        {
            get => this._cam1SingleView;
            set => SetProperty(ref this._cam1SingleView, value);
        }

        private List<string> dateTimes = new List<string>();
        public List<string> DateTimes
        {
            get => this.dateTimes;
            set => SetProperty(ref this.dateTimes, value);
        }

        AlignHistoryView _cam1AlignHistoryView = null;
        public AlignHistoryView Cam1AliginHistoryView { get => this._cam1AlignHistoryView; set => SetProperty(ref this._cam1AlignHistoryView, value); }

        LogListView _cam1LogListView = null;
        public LogListView Cam1LogListView { get => this._cam1LogListView; set => SetProperty(ref this._cam1LogListView, value); }

        PLCInterfaceHTypeView _cam1PLCPIOView = null;
        public PLCInterfaceHTypeView Cam1PLCPIOView { get => this._cam1PLCPIOView; set => SetProperty(ref this._cam1PLCPIOView, value); }

        private string targetModel;
        public string TargetModel { get => this.targetModel; set => SetProperty(ref this.targetModel, value); }

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

        SeriesCollection resultCountSeries;
        public SeriesCollection ResultCountSeries { get => this.resultCountSeries; set => SetProperty(ref this.resultCountSeries, value); }

        SeriesCollection refreshCountSeries;
        public SeriesCollection RefreshCountSeries { get => this.refreshCountSeries; set => SetProperty(ref this.refreshCountSeries, value); }

        eSystemState systemState = eSystemState.None;
        public eSystemState SystemState { get => this.systemState; set => SetProperty(ref this.systemState, value); }

        private bool _isExistMesh1;
        public bool IsExistMesh1
        {
            get { return _isExistMesh1; }
            set { SetProperty(ref _isExistMesh1, value); }
        }

        private bool _isExistMesh2;
        public bool IsExistMesh2
        {
            get { return _isExistMesh2; }
            set { SetProperty(ref _isExistMesh2, value); }
        }

        private bool _isExistMesh3;
        public bool IsExistMesh3
        {
            get { return _isExistMesh3; }
            set { SetProperty(ref _isExistMesh3, value); }
        }

        private bool _isExistMesh4;
        public bool IsExistMesh4
        {
            get { return _isExistMesh4; }
            set { SetProperty(ref _isExistMesh4, value); }
        }

        private bool _isExistMesh5;
        public bool IsExistMesh5
        {
            get { return _isExistMesh5; }
            set { SetProperty(ref _isExistMesh5, value); }
        }

        private bool _isExistMesh6;
        public bool IsExistMesh6
        {
            get { return _isExistMesh6; }
            set { SetProperty(ref _isExistMesh6, value); }
        }

        private bool _isOverlapMesh1;
        public bool IsOverlapMesh1
        {
            get { return _isOverlapMesh1; }
            set { SetProperty(ref _isOverlapMesh1, value); }
        }

        private bool _isOverlapMesh2;
        public bool IsOverlapMesh2
        {
            get { return _isOverlapMesh2; }
            set { SetProperty(ref _isOverlapMesh2, value); }
        }

        private bool _isOverlapMesh3;
        public bool IsOverlapMesh3
        {
            get { return _isOverlapMesh3; }
            set { SetProperty(ref _isOverlapMesh3, value); }
        }

        private bool _isOverlapMesh4;
        public bool IsOverlapMesh4
        {
            get { return _isOverlapMesh4; }
            set { SetProperty(ref _isOverlapMesh4, value); }
        }

        private bool _isOverlapMesh5;
        public bool IsOverlapMesh5
        {
            get { return _isOverlapMesh5; }
            set { SetProperty(ref _isOverlapMesh5, value); }
        }

        private bool _isOverlapMesh6;
        public bool IsOverlapMesh6
        {
            get { return _isOverlapMesh6; }
            set { SetProperty(ref _isOverlapMesh6, value); }
        }
        IContainerProvider provider = null;
        IRegionManager regionManager = null;
        IEventAggregator _eventAggregator = null;
        GUIMessageEvent guiEventPublisher = null;

        IContainerProvider _provider = null;
        SqlManager sql = null;
        bool _isInited = false;

        #endregion

        #region ICommands
        public ICommand SystemStateChangeCommand { get; set; }
        public ICommand ChangeModelCommand { get; set; }
        public ICommand EditModelCommand { get; set; }
        #endregion

        #region Struct
        public MLCCMainViewModel(IContainerProvider provider, IContainerProvider ioc, IEventAggregator aggregator, SqlManager sqlManager)
        {
            this._provider = ioc;
            this._eventAggregator = aggregator;
            this.sql = sqlManager;
            this.provider = provider;

            this._eventAggregator.GetEvent<ApplicationExitEvent>().Subscribe((o) => Dispose(), true);

            this._eventAggregator.GetEvent<CoreMessageEvent>().Unsubscribe(UICallbackCommunication);
            this._eventAggregator.GetEvent<CoreMessageEvent>().Subscribe(UICallbackCommunication, ThreadOption.UIThread);

            this._eventAggregator.GetEvent<GUIMessageEvent>().Unsubscribe(CoreCallbackCommunication);
            this._eventAggregator.GetEvent<GUIMessageEvent>().Subscribe(CoreCallbackCommunication, ThreadOption.UIThread);

            guiEventPublisher = this._eventAggregator.GetEvent<GUIMessageEvent>();

            InitDelegeteCommand();

        }
        public void Init()
        {
            if (_isInited) return;
            _isInited = true;

            SystemState = eSystemState.Auto;

            this.CurrentModelName = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel.Name;
            this.TargetModel = this.CurrentModelName;
            ModelList = sql.ModelData.GetAll().Select(x => x.Name).ToList();

            this.Cam1SingleView = _provider.Resolve<CogDisplaySingleView>();
            this.Cam1SingleView.ViewModel.camID = Common.Shared.eCamID.Cam1;

            this.Cam1LogListView = _provider.Resolve<LogListView>();
            this.Cam1LogListView.ViewModel.Zone = Common.Shared.eExecuteZone.MLCC_INSPECTION;

            this.Cam1PLCPIOView = _provider.Resolve<PLCInterfaceHTypeView>();
            this.Cam1PLCPIOView.ViewModel.SubText = "";

            this.ResultCountSeries = new SeriesCollection();
            this.ResultCountSeries.Add(new ColumnSeries { Title = "Count", Name = "Count", Values = new ChartValues<ObservableValue>(), Fill = Brushes.Orange, Stroke = Brushes.Orange });

            RefreshChart();

            GSG.NET.Quartz.QuartzUtils.Invoke("CHART_REFRESH", GSG.NET.Quartz.QuartzUtils.GetExpnDay(0, 0, 0), RefreshChart);
        }
        void InitDelegeteCommand()
        {
            this.SystemStateChangeCommand = new DelegateCommand<string>(ExecuteSystemStateChangeCommand);

            this.ChangeModelCommand = new DelegateCommand(ExecuteChangeModelCommand);
            this.EditModelCommand = new DelegateCommand(ExecuteEditModelCommand);

        }

        void Dispose()
        {
            GSG.NET.Quartz.QuartzUtils.StopSchedule("CHART_REFRESH");
        }

        #endregion

        #region UI Call Back Communication
        private void CoreCallbackCommunication(GUIEventArgs obj)
        {
            switch (obj.MessageKind)
            {
                case eGUIMessageKind.ChangeModelList:
                    this.ModelList = sql.ModelData.GetAll().Select(x => x.Name).ToList();
                    break;
                default:
                    break;
            }
        }
        private void UICallbackCommunication(CoreEventArgs obj)
        {
            switch (obj.MessageKind)
            {
                case eCoreMessageKind.ResultChange:
                    OnReceiveResultChange(obj);
                    break;
                case eCoreMessageKind.ChangeModelList:
                    this.ModelList = sql.ModelData.GetAll().Select(x => x.Name).ToList();
                    break;
                default:
                    break;
            }
        }
        private void OnReceiveResultChange(CoreEventArgs obj)
        {
            var msg = obj.Arg as ResultMLCCInspection;

            var ResultList = msg.ResultMLCCItems;

            foreach (ResultMLCCItem item in ResultList)
            {
                var existInspection = item.IsExistence.Equals(true) ? true : false;
                var overLapInspection = item.IsOverlap.Equals(true) ? true : false;

                switch (item.MeshID)
                {
                    case eMesh.MESH1:
                        this.IsExistMesh1 = existInspection;
                        this.IsOverlapMesh1 = overLapInspection;
                        break;

                    case eMesh.MESH2:
                        this.IsExistMesh2 = existInspection;
                        this.IsOverlapMesh2 = overLapInspection;
                        break;

                    case eMesh.MESH3:
                        this.IsExistMesh3 = existInspection;
                        this.IsOverlapMesh3 = overLapInspection;
                        break;

                    case eMesh.MESH4:
                        this.IsExistMesh4 = existInspection;
                        this.IsOverlapMesh4 = overLapInspection;
                        break;

                    case eMesh.MESH5:
                        this.IsExistMesh5 = existInspection;
                        this.IsOverlapMesh5 = overLapInspection;
                        break;

                    case eMesh.MESH6:
                        this.IsExistMesh6 = existInspection;
                        this.IsOverlapMesh6 = overLapInspection;
                        break;
                }
            }
        }
        #endregion

        #region Command
        private void ExecuteSystemStateChangeCommand(string obj)
        {
            this.SystemState = obj.Equals("Auto") ? eSystemState.Auto : eSystemState.Manual;
            var msg = new GUIEventArgs()
            {
                MessageKind = eGUIMessageKind.SystemStateChange,
                Arg = this.SystemState,
            };

            this.guiEventPublisher.Publish(msg);

            //if (this.SystemState == eSystemState.Auto)
            //{
            //    regionManager.RequestNavigate(RegionNames.MainView, UISelecter.Instance.AutoViewName);
            //}
        }
        private async void ExecuteChangeModelCommand()
        {
            var view = this.provider.Resolve<ComfirmationView>();
            view.ViewModel.Message = $"Model Change to {this.TargetModel} ?";

            var result = await DialogHost.Show(view, "RootDialog") as bool?;
            if (result == true)
            {
                var sys = sql.SystemInfo.GetAll().FirstOrDefault();
                var targetMode = sql.ModelData.FindBy(x => x.Name.Equals(TargetModel)).FirstOrDefault();
                sys.CurrentProductModelId = targetMode.Id;
                sql.SystemInfo.Edit(sys);
                CurrentModelName = targetMode.Name;

                var msg = new GUIEventArgs()
                {
                    MessageKind = eGUIMessageKind.ModelChange,
                    Arg = CurrentModelName,
                };

                this.guiEventPublisher.Publish(msg);
            }
        }
        private async void ExecuteEditModelCommand()
        {
            var view = this.provider.Resolve<ModelManagerView>();
            await DialogHost.Show(view, "RootDialog");
        }

        #endregion

        #region Private Method
        private void RefreshChart()
        {
            if (ResultCountSeries[0].Values.Count != 0)
                ResultCountSeries[0].Values.Clear();

            if (DateTimes.Count != 0)
                DateTimes.Clear();

            var refreshDate = new List<string>();

            // today -5 history data
            for (int i = -5; i < 0; i++)
            {
                var day = DateTime.Now.AddDays(i).ToString("yyyy-MM-dd");

                refreshDate.Add(DateTime.Now.AddDays(i).ToString("MM-dd"));

                DateTimes = refreshDate;

                var dayLog = this.sql.InspectionHistory.GetAll().Where(x => x.CreateDate.ToString("yyyy-MM-dd").Equals(day)).ToList();
                var dayLogCount = dayLog.Count;

                ResultCountSeries[0].Values.Add(new ObservableValue { Value = dayLogCount });
            }
        }
        #endregion
    }
}
