using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.PMAlign;
using GSG.NET.Concurrent;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows.Forms;
using System.Windows.Input;
using VASFx.Common.Events;
using VASFx.Common.Interface;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.Core;
using VASFx.MLCC.Sqlite;
using VASFx.UI.CogDisplayViews.Views;
using VASFx.UI.Interactivity;
using VASFx.UI.LightControlViews.UI;
using VASFx.VisionLibrary.Cognex;

namespace VASFx.UI.VisionEditorViews.VisionEditor
{
    public class BlobToolsEditorViewModel : BindableBase
    {
        #region Properties

        CogDisplayView cogDisplay = null;
        public CogDisplayView CogDisplay { get => this.cogDisplay; set => SetProperty(ref this.cogDisplay, value); }

        SettingLightView settingLigthView_1 = null;
        public SettingLightView SettingLigthView_1 { get => this.settingLigthView_1; set => SetProperty(ref this.settingLigthView_1, value); }

        SettingLightView settingLigthView_2 = null;
        public SettingLightView SettingLigthView_2 { get => this.settingLigthView_2; set => SetProperty(ref this.settingLigthView_2, value); }

        SettingLightView settingLigthView_3 = null;
        public SettingLightView SettingLigthView_3 { get => this.settingLigthView_3; set => SetProperty(ref this.settingLigthView_3, value); }

        SettingLightView settingLigthView_4 = null;
        public SettingLightView SettingLigthView_4 { get => this.settingLigthView_4; set => SetProperty(ref this.settingLigthView_4, value); }

        private ObservableCollection<ListBoxItem> segmentationParams;
        public ObservableCollection<ListBoxItem> SegmentationParams
        {
            get { return segmentationParams; }
            set { SetProperty(ref this.segmentationParams, value); }
        }

        private ObservableCollection<ListBoxItem> connectivityParams;
        public ObservableCollection<ListBoxItem> ConnectivityParams
        {
            get { return connectivityParams; }
            set { SetProperty(ref this.connectivityParams, value); }
        }

        private ObservableCollection<ListBoxItem> trainRegionParam;
        public ObservableCollection<ListBoxItem> TrainRegionParam
        {
            get { return trainRegionParam; }
            set { SetProperty(ref this.trainRegionParam, value); }
        }

        private ObservableCollection<ListBoxItem> trainOriginParam;
        public ObservableCollection<ListBoxItem> TrainOriginParam
        {
            get { return trainOriginParam; }
            set { SetProperty(ref this.trainOriginParam, value); }
        }

        private ObservableCollection<ListItem> morphologyParam;
        public ObservableCollection<ListItem> MorphologyParam
        {
            get { return morphologyParam; }
            set { SetProperty(ref this.morphologyParam, value); }
        }

        private List<string> imageSourceList;
        public List<string> ImageSourceList
        {
            get { return imageSourceList; }
            set { SetProperty(ref this.imageSourceList, value); }
        }

        private string imageSourceSelectedItem;
        public string ImageSourceSelectedItem
        {
            get { return imageSourceSelectedItem; }
            set
            {
                if (SetProperty(ref this.imageSourceSelectedItem, value))
                {
                    ChangeImageSource(imageSourceSelectedItem);
                }
            }
        }

        private CogBlobSegmentationModeConstants segmentationModeItem;
        public CogBlobSegmentationModeConstants SegmentationModeItem
        {
            get { return segmentationModeItem; }
            set
            {
                if (SetProperty(ref this.segmentationModeItem, value))
                {
                    SetSegmentationMode(segmentationModeItem);
                }
            }
        }
        private List<CogBlobSegmentationModeConstants> segmentationModeList;
        public List<CogBlobSegmentationModeConstants> SegmentationModeList
        {
            get { return segmentationModeList; }
            set { SetProperty(ref this.segmentationModeList, value); }
        }

        private CogBlobSegmentationPolarityConstants polarityConstantsItem;
        public CogBlobSegmentationPolarityConstants PolarityConstantsItem
        {
            get { return polarityConstantsItem; }
            set
            {
                if (SetProperty(ref this.polarityConstantsItem, value))
                {
                    SetPolarityMode(polarityConstantsItem);
                }
            }
        }
        private List<CogBlobSegmentationPolarityConstants> polarityConstantsList;
        public List<CogBlobSegmentationPolarityConstants> PolarityConstantsList
        {
            get { return polarityConstantsList; }
            set { SetProperty(ref this.polarityConstantsList, value); }
        }

        private CogBlobConnectivityModeConstants connectivityModeConstantsItem = CogBlobConnectivityModeConstants.Labeled;
        public CogBlobConnectivityModeConstants ConnectivityModeConstantsItem
        {
            get { return connectivityModeConstantsItem; }
            set
            {
                if (SetProperty(ref this.connectivityModeConstantsItem, value))
                {
                    SetConnectivityMode(connectivityModeConstantsItem);
                }
            }
        }

        private List<CogBlobConnectivityModeConstants> connectivityModeConstantsList;
        public List<CogBlobConnectivityModeConstants> ConnectivityModeConstantsList
        {
            get { return connectivityModeConstantsList; }
            set { SetProperty(ref this.connectivityModeConstantsList, value); }
        }

        private CogBlobConnectivityCleanupConstants connectivityCleanupConstantsItem;
        public CogBlobConnectivityCleanupConstants ConnectivityCleanupConstantsItem
        {
            get { return connectivityCleanupConstantsItem; }
            set
            {
                if (SetProperty(ref this.connectivityCleanupConstantsItem, value))
                {
                    SetCleanup(connectivityCleanupConstantsItem);
                }
            }
        }

        private List<CogBlobConnectivityCleanupConstants> connectivityCleanupConstantsList;
        public List<CogBlobConnectivityCleanupConstants> ConnectivityCleanupConstantsList
        {
            get { return connectivityCleanupConstantsList; }
            set { SetProperty(ref this.connectivityCleanupConstantsList, value); }
        }

        private CogBlobMorphologyConstants morphologyModeItem;
        public CogBlobMorphologyConstants MorphologyModeItem
        {
            get { return morphologyModeItem; }
            set
            {
                if (SetProperty(ref this.morphologyModeItem, value))
                {
                    //SetMorphologyMode();
                }
            }
        }

        private List<CogBlobMorphologyConstants> morphologyModeList;
        public List<CogBlobMorphologyConstants> MorphologyModeList
        {
            get { return morphologyModeList; }
            set { SetProperty(ref this.morphologyModeList, value); }
        }

        private string regionItem;
        public string RegionItem
        {
            get { return regionItem; }
            set
            {
                if (SetProperty(ref this.regionItem, value))
                {
                    SetTrainRegionOriginData(RegionItem);
                }
            }
        }

        private List<string> regionList;
        public List<string> RegionList
        {
            get { return regionList; }
            set { SetProperty(ref this.regionList, value); }
        }

        private List<BlobResult> blobResultList;
        public List<BlobResult> BlobResultList
        {
            get { return blobResultList; }
            set { SetProperty(ref this.blobResultList, value); }
        }
        private bool isSaving;
        public bool IsSaving { get => this.isSaving; set => SetProperty(ref this.isSaving, value); }

        public eExecuteZone eExecuteZone { get; set; }
        public eGrabPosition eGrabPosition { get; set; }
        public eBlobNum BlobID { get; set; }
        public ePatternNum PatternID { get; set; }

        public ICogImage inputImage { get; set; }
        public ICogImage trainImage { get; set; }
        public ICogImage segmentedImage { get; set; }
        public ICogImage outputImage { get; set; }

        IContainerProvider provider = null;
        CameraManager cameraManager = null;
        SqlManager sql = null;
        ICamera camera = null;

        CogBlobTool cogBlobTool = null;
        CogPMAlignTool cogPMAlignTool = null;
        CogBlob CogBlobParams = null;
        CogRectangle cogTrainRegion = null;
        CogVisionPro lib = null;

        GUIMessageEvent _gUIMessageEvent = null;
        IEventAggregator _eventAggregator = null;
        #endregion

        #region Command

        public ICommand SaveImageCommand { get; set; }
        public ICommand LoadImageCommand { get; set; }
        public ICommand GrabCommand { get; set; }
        public ICommand LiveCommand { get; set; }
        public ICommand TrainCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand AddMorphologyCommand { get; set; }
        public ICommand DeleteMorphologyCommand { get; set; }
        public ICommand FitInImageCommand { get; set; }
        public ICommand RunBlobToolCommand { get; set; }
        public ICommand ElectricModeCommand { get; set; }
        #endregion

        #region Struct
        public BlobToolsEditorViewModel(IEventAggregator eventAggregator, IContainerProvider prov, CameraManager cameraManager, CogVisionPro lib, SqlManager sql)
        {
            this.provider = prov;
            this.cameraManager = cameraManager;
            this.sql = sql;
            this.lib = lib;
            this._eventAggregator = eventAggregator;

            this.CogDisplay = provider.Resolve<CogDisplayView>();

            var controlloer = sql.LightController.GetAll().FirstOrDefault();

            this.SettingLigthView_1 = provider.Resolve<SettingLightView>();
            SettingLigthView_1.ViewModel.PortId = controlloer.PortNumber;
            SettingLigthView_1.ViewModel.Channel = 1;

            this.SettingLigthView_2 = provider.Resolve<SettingLightView>();
            SettingLigthView_2.ViewModel.PortId = controlloer.PortNumber;
            SettingLigthView_2.ViewModel.Channel = 2;

            //Moon
            this.SettingLigthView_3 = provider.Resolve<SettingLightView>();
            SettingLigthView_3.ViewModel.PortId = controlloer.PortNumber;
            SettingLigthView_3.ViewModel.Channel = 3;

            this.SettingLigthView_4 = provider.Resolve<SettingLightView>();
            SettingLigthView_4.ViewModel.PortId = controlloer.PortNumber;
            SettingLigthView_4.ViewModel.Channel = 4;
            //
            this._gUIMessageEvent = this._eventAggregator.GetEvent<GUIMessageEvent>();
            InitializeCommand();
        }
        public void Init()
        {
            eExecuteZone = eExecuteZone.MLCC_INSPECTION;
            eGrabPosition = eGrabPosition.First;
            BlobID = eBlobNum.Blob1;
            PatternID = ePatternNum.Pattern1;

            this.camera = cameraManager.Cameras.Values.FirstOrDefault(_ => _.CamID == eCamID.Cam1);

            cogBlobTool = cameraManager.CogJobs[eExecuteZone][eGrabPosition].BlobDic[BlobID];
            cogPMAlignTool = cameraManager.CogJobs[eExecuteZone][eGrabPosition].PMAlignDic[PatternID];

            if (ImageSourceList == null)
            {
                ImageSourceList = new List<string>();
                ImageSourceList.Add("InputImage");
                ImageSourceList.Add("TrainImage");
                ImageSourceList.Add("OutputImage");
            }

            ImageSourceSelectedItem = "InputImage";

            if (cogPMAlignTool.Pattern.TrainImage != null)
                trainImage = cogPMAlignTool.Pattern.TrainImage;

            GetBlobParams();
            GetRegionOriginData();

            this.CogDisplay.FixAirspace = false;
            //this.CogPatternDisplay.FixAirspace = false;
        }

        private void InitializeCommand()
        {
            SaveImageCommand = new DelegateCommand(ExecuteImageSaveCommand);
            LoadImageCommand = new DelegateCommand(ExecuteImageLoadCommand);

            GrabCommand = new DelegateCommand(ExcuteGrabCommand);
            TrainCommand = new DelegateCommand(ExcuteTrainCommand);
            SaveCommand = new DelegateCommand(ExcuteSaveCommand);
            RunBlobToolCommand = new DelegateCommand(ExcuteRunBlobToolCommand);

            AddMorphologyCommand = new DelegateCommand(ExcuteAddMorphologyCommand);
            DeleteMorphologyCommand = new DelegateCommand(ExcuteDeleteMorphologyCommand);

            FitInImageCommand = new DelegateCommand(ExcuteFitInImageCommand);
        }

        #endregion

        #region CogDisplay

        private void ChangeImageSource(string imageSourceSelectedItem)
        {
            switch (imageSourceSelectedItem)
            {
                case "InputImage":
                    if (inputImage != null)
                        CogDisplay.SetImage(inputImage);

                    CogDisplay.ClearGraphics();

                    break;
                case "TrainImage":
                    if (trainImage != null)
                        CogDisplay.SetImage(trainImage);

                    CogDisplay.ClearGraphics();

                    GetTrainRegionOriginGraphic();

                    break;
                case "OutputImage":
                    if (outputImage != null)
                    {
                        CogDisplay.SetImage(outputImage);

                        CogDisplay.ClearGraphics();

                        for (int i = 0; i < BlobResultList.Count; i++)
                        {
                            GetBlobRunResultGraphic(BlobResultList[i].BlobGraphic);
                        }
                    }

                    break;

                default:
                    //CogDisplay.ViewModel.ClearImage();

                    //CogDisplay.ViewModel.ClearGraphics();

                    break;
            }
        }
        private void GetBlobRunResultGraphic(CogPolygon cogPolygon)
        {
            var graphic = new CogGraphicInteractiveCollection();
            graphic.Add(cogPolygon);
            CogDisplay.SetGraphic(graphic, "BlobResult", true);
        }

        #endregion

        #region BlobParamsControl

        private void GetBlobParams()
        {
            CogBlobParams = cogBlobTool.RunParams;

            if (SegmentationModeList == null)
            {
                SegmentationModeList = new List<CogBlobSegmentationModeConstants>();
                SegmentationModeList.Add(CogBlobSegmentationModeConstants.HardFixedThreshold);
                SegmentationModeList.Add(CogBlobSegmentationModeConstants.HardRelativeThreshold);
                SegmentationModeList.Add(CogBlobSegmentationModeConstants.HardDynamicThreshold);
                SegmentationModeList.Add(CogBlobSegmentationModeConstants.SoftFixedThreshold);
                SegmentationModeList.Add(CogBlobSegmentationModeConstants.SoftRelativeThreshold);
            }

            if (PolarityConstantsList == null)
            {
                PolarityConstantsList = new List<CogBlobSegmentationPolarityConstants>();
                PolarityConstantsList.Add(CogBlobSegmentationPolarityConstants.LightBlobs);
                PolarityConstantsList.Add(CogBlobSegmentationPolarityConstants.DarkBlobs);
            }

            if (ConnectivityModeConstantsList == null)
            {
                ConnectivityModeConstantsList = new List<CogBlobConnectivityModeConstants>();
                ConnectivityModeConstantsList.Add(CogBlobConnectivityModeConstants.GreyScale);
                ConnectivityModeConstantsList.Add(CogBlobConnectivityModeConstants.Labeled);
                ConnectivityModeConstantsList.Add(CogBlobConnectivityModeConstants.WholeImageGreyScale);
            }

            if (ConnectivityCleanupConstantsList == null)
            {
                ConnectivityCleanupConstantsList = new List<CogBlobConnectivityCleanupConstants>();
                ConnectivityCleanupConstantsList.Add(CogBlobConnectivityCleanupConstants.None);
                ConnectivityCleanupConstantsList.Add(CogBlobConnectivityCleanupConstants.Prune);
                ConnectivityCleanupConstantsList.Add(CogBlobConnectivityCleanupConstants.Fill);
            }

            if (MorphologyModeList == null)
            {
                MorphologyModeList = new List<CogBlobMorphologyConstants>();
                MorphologyModeList.Add(CogBlobMorphologyConstants.ErodeHorizontal);
                MorphologyModeList.Add(CogBlobMorphologyConstants.ErodeVertical);
                MorphologyModeList.Add(CogBlobMorphologyConstants.ErodeSquare);
                MorphologyModeList.Add(CogBlobMorphologyConstants.DilateHorizontal);
                MorphologyModeList.Add(CogBlobMorphologyConstants.DilateVertical);
                MorphologyModeList.Add(CogBlobMorphologyConstants.DilateSquare);
                MorphologyModeList.Add(CogBlobMorphologyConstants.CloseHorizontal);
                MorphologyModeList.Add(CogBlobMorphologyConstants.CloseVertical);
                MorphologyModeList.Add(CogBlobMorphologyConstants.CloseSquare);
                MorphologyModeList.Add(CogBlobMorphologyConstants.OpenHorizontal);
                MorphologyModeList.Add(CogBlobMorphologyConstants.OpenVertical);
                MorphologyModeList.Add(CogBlobMorphologyConstants.OpenSquare);
            }

            this.SegmentationModeItem = SegmentationModeList.FirstOrDefault(x => x.Equals(CogBlobParams.SegmentationParams.Mode));
            this.PolarityConstantsItem = PolarityConstantsList.FirstOrDefault(x => x.Equals(CogBlobParams.SegmentationParams.Polarity));
            this.ConnectivityModeConstantsItem = ConnectivityModeConstantsList.FirstOrDefault(x => x.Equals(CogBlobParams.ConnectivityMode));
            this.ConnectivityCleanupConstantsItem = ConnectivityCleanupConstantsList.FirstOrDefault(x => x.Equals(CogBlobParams.ConnectivityCleanup));
            this.MorphologyModeItem = MorphologyModeList.FirstOrDefault(x => x.Equals(CogBlobMorphologyConstants.ErodeSquare));

            SetSegmentationMode(SegmentationModeItem);
            SetPolarityMode(PolarityConstantsItem);
            SetConnectivityMode(ConnectivityModeConstantsItem);
            SetCleanup(ConnectivityCleanupConstantsItem);
            SetMorphologyMode();
        }
        private void SetSegmentationMode(CogBlobSegmentationModeConstants mode)
        {
            cogBlobTool.RunParams.SegmentationParams.Mode = mode;

            switch (mode)
            {
                case CogBlobSegmentationModeConstants.HardFixedThreshold:
                    SegmentationParams = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    SegmentationParams.Add(new ListBoxItem() { Name = "Threshold", Value = CogBlobParams.SegmentationParams.HardFixedThreshold.ToString() });
                    break;
                case CogBlobSegmentationModeConstants.HardRelativeThreshold:
                    SegmentationParams = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    SegmentationParams.Add(new ListBoxItem() { Name = "Threshold", Value = CogBlobParams.SegmentationParams.HardRelativeThreshold.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "LowTail", Value = CogBlobParams.SegmentationParams.TailLow.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "HighTail", Value = CogBlobParams.SegmentationParams.TailHigh.ToString() });
                    break;
                case CogBlobSegmentationModeConstants.HardDynamicThreshold:
                    SegmentationParams = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    SegmentationParams.Add(new ListBoxItem() { Name = "LowTail", Value = CogBlobParams.SegmentationParams.TailLow.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "HighTail", Value = CogBlobParams.SegmentationParams.TailHigh.ToString() });
                    break;
                case CogBlobSegmentationModeConstants.SoftFixedThreshold:
                    SegmentationParams = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    SegmentationParams.Add(new ListBoxItem() { Name = "LowThreshold", Value = cogBlobTool.RunParams.SegmentationParams.SoftFixedThresholdLow.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "HighThreshold", Value = cogBlobTool.RunParams.SegmentationParams.SoftFixedThresholdHigh.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "Softness", Value = cogBlobTool.RunParams.SegmentationParams.Softness.ToString() });
                    break;
                case CogBlobSegmentationModeConstants.SoftRelativeThreshold:
                    SegmentationParams = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    SegmentationParams.Add(new ListBoxItem() { Name = "LowThreshold", Value = cogBlobTool.RunParams.SegmentationParams.SoftFixedThresholdLow.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "HighThreshold", Value = cogBlobTool.RunParams.SegmentationParams.SoftFixedThresholdHigh.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "LowTail", Value = cogBlobTool.RunParams.SegmentationParams.TailLow.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "HighTail", Value = cogBlobTool.RunParams.SegmentationParams.TailHigh.ToString() });
                    SegmentationParams.Add(new ListBoxItem() { Name = "Softness", Value = cogBlobTool.RunParams.SegmentationParams.Softness.ToString() });
                    break;
            }
            this.SegmentationParams.ToList().ForEach(item => { item.OnValueChanged += SegmentationDataChanged; });
        }

        private void SetPolarityMode(CogBlobSegmentationPolarityConstants mode)
        {
            cogBlobTool.RunParams.SegmentationParams.Polarity = mode;
        }
        private void SetConnectivityMode(CogBlobConnectivityModeConstants mode)
        {
            cogBlobTool.RunParams.ConnectivityMode = mode;
        }
        private void SetCleanup(CogBlobConnectivityCleanupConstants mode)
        {
            cogBlobTool.RunParams.ConnectivityCleanup = mode;

            switch (mode)
            {
                case CogBlobConnectivityCleanupConstants.None:
                    ConnectivityParams = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    ConnectivityParams.Add(new ListBoxItem() { Name = "MinArea", Value = cogBlobTool.RunParams.ConnectivityMinPixels.ToString() });
                    break;
                case CogBlobConnectivityCleanupConstants.Prune:
                    ConnectivityParams = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    ConnectivityParams.Add(new ListBoxItem() { Name = "MinArea", Value = cogBlobTool.RunParams.ConnectivityMinPixels.ToString() });
                    break;
                case CogBlobConnectivityCleanupConstants.Fill:
                    ConnectivityParams = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    ConnectivityParams.Add(new ListBoxItem() { Name = "MinArea", Value = cogBlobTool.RunParams.ConnectivityMinPixels.ToString() });
                    break;
            }

            this.ConnectivityParams.ToList().ForEach(item => { item.OnValueChanged += ConnectivityDataChanged; });
        }
        private void SetMorphologyMode()
        {
            //cogBlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.CloseHorizontal);
            //cogBlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.OpenSquare);
            //cogBlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeHorizontal);

            CogBlobMorphologyCollection blobMorphology = cogBlobTool.RunParams.MorphologyOperations;

            MorphologyParam = new System.Collections.ObjectModel.ObservableCollection<ListItem>();

            foreach (CogBlobMorphologyConstants item in blobMorphology)
            {
                MorphologyParam.Add(new ListItem() { Name = item.ToString() });
            }

        }

        private void SegmentationDataChanged(object sender, dynamic value)
        {
            var mode = cogBlobTool.RunParams.SegmentationParams.Mode;
            var segmentationData = cogBlobTool.RunParams.SegmentationParams;

            switch (mode)
            {
                case CogBlobSegmentationModeConstants.HardFixedThreshold:
                    segmentationData.HardFixedThreshold = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("Threshold")).Value);
                    break;
                case CogBlobSegmentationModeConstants.HardRelativeThreshold:
                    segmentationData.HardRelativeThreshold = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("Threshold")).Value);
                    segmentationData.TailLow = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("LowTail")).Value);
                    segmentationData.TailHigh = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("HighTail")).Value);
                    break;
                case CogBlobSegmentationModeConstants.HardDynamicThreshold:
                    segmentationData.TailLow = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("LowTail")).Value);
                    segmentationData.TailHigh = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("HighTail")).Value);
                    break;
                case CogBlobSegmentationModeConstants.SoftFixedThreshold:
                    segmentationData.SoftFixedThresholdLow = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("LowThreshold")).Value);
                    segmentationData.SoftFixedThresholdHigh = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("HighThreshold")).Value);
                    segmentationData.Softness = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("Softness")).Value);
                    break;
                case CogBlobSegmentationModeConstants.SoftRelativeThreshold:
                    segmentationData.SoftFixedThresholdLow = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("LowThreshold")).Value);
                    segmentationData.SoftFixedThresholdHigh = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("HighThreshold")).Value);
                    segmentationData.Softness = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("Softness")).Value);
                    segmentationData.TailLow = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("LowTail")).Value);
                    segmentationData.TailHigh = int.Parse(this.SegmentationParams.FirstOrDefault(t => t.Name.Equals("HighTail")).Value);
                    break;
            }

            cogBlobTool.RunParams.SegmentationParams = segmentationData;

        }
        private void ConnectivityDataChanged(object sender, dynamic value)
        {
            var connectivityParams = cogBlobTool.RunParams.ConnectivityMinPixels;

            connectivityParams = int.Parse(this.ConnectivityParams.FirstOrDefault(t => t.Name.Equals("MinArea")).Value);

            cogBlobTool.RunParams.ConnectivityMinPixels = connectivityParams;

        }
        #endregion

        #region Train Region, Origin

        private void GetRegionOriginData()
        {

            if (RegionList == null)
            {
                RegionList = new List<string>();
                RegionList.Add("Rectangle");
            }
            RegionItem = RegionList.FirstOrDefault(x => x.Equals("Rectangle"));

            SetTrainRegionOriginData(RegionItem);
        }
        private void SetTrainRegionOriginData(string RegionType)
        {
            cogTrainRegion = (CogRectangle)cogBlobTool.Region;

            if (cogTrainRegion == null)
            {
                var serchRegion = new CogRectangle();
                //serchRegion.SetXYWidthHeight(0, 0, inputImage.Width, inputImage.Height);
                serchRegion.SetXYWidthHeight(0, 0, 3840, 2748);
                serchRegion.GraphicDOFEnable = CogRectangleDOFConstants.Size | CogRectangleDOFConstants.Position;
                serchRegion.Interactive = true;
                cogTrainRegion = serchRegion;
                cogBlobTool.Region = serchRegion;
            }

            switch (RegionType)
            {
                case "None":
                    break;
                case "Rectangle":
                    TrainRegionParam = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                    TrainRegionParam.Add(new ListBoxItem() { Name = "CenterX", Value = cogTrainRegion.CenterX.ToString("F") });
                    TrainRegionParam.Add(new ListBoxItem() { Name = "CenterY", Value = cogTrainRegion.CenterY.ToString("F") });
                    TrainRegionParam.Add(new ListBoxItem() { Name = "Width", Value = cogTrainRegion.Width.ToString("F") });
                    TrainRegionParam.Add(new ListBoxItem() { Name = "Height", Value = cogTrainRegion.Height.ToString("F") });

                    TrainOriginParam = new ObservableCollection<ListBoxItem>();
                    TrainOriginParam.Add(new ListBoxItem() { Name = "OriginX", Value = cogTrainRegion.X.ToString("F") });
                    TrainOriginParam.Add(new ListBoxItem() { Name = "OriginY", Value = cogTrainRegion.Y.ToString("F") });
                    TrainOriginParam.Add(new ListBoxItem() { Name = "Width", Value = cogTrainRegion.Width.ToString("F") });
                    TrainOriginParam.Add(new ListBoxItem() { Name = "Height", Value = cogTrainRegion.Height.ToString("F") });
                    break;
            }

            this.TrainRegionParam.ToList().ForEach(item => { item.OnValueChanged += SetTrainRegionCenterDataChanged; });
            this.TrainOriginParam.ToList().ForEach(item => { item.OnValueChanged += SetTrainRegionOriginDataChanged; });

        }
        private void SetTrainRegionOriginDataChanged(object sender, EventArgs e)
        {
            var region = new CogRectangle();

            var originX = Convert.ToDouble(this.TrainOriginParam.FirstOrDefault(t => t.Name.Equals("OriginX")).Value);
            var originY = Convert.ToDouble(this.TrainOriginParam.FirstOrDefault(t => t.Name.Equals("OriginY")).Value);
            var width = Convert.ToDouble(this.TrainRegionParam.FirstOrDefault(t => t.Name.Equals("Width")).Value);
            var height = Convert.ToDouble(this.TrainRegionParam.FirstOrDefault(t => t.Name.Equals("Height")).Value);

            region.SetXYWidthHeight(originX, originY, width, height);
            region.GraphicDOFEnable = CogRectangleDOFConstants.Size | CogRectangleDOFConstants.Position;
            region.Interactive = true;

            cogBlobTool.Region = region;
            cogTrainRegion = region;

            TrainRegionParam.Where(x => x.Name.Equals("CenterX")).FirstOrDefault().Value = region.CenterX;
            TrainRegionParam.Where(x => x.Name.Equals("CenterY")).FirstOrDefault().Value = region.CenterY;
            TrainRegionParam.Where(x => x.Name.Equals("Width")).FirstOrDefault().Value = region.Width;
            TrainRegionParam.Where(x => x.Name.Equals("Height")).FirstOrDefault().Value = region.Height;

            TrainOriginParam.Where(x => x.Name.Equals("Width")).FirstOrDefault().Value = region.Width;
            TrainOriginParam.Where(x => x.Name.Equals("Height")).FirstOrDefault().Value = region.Height;

            GetTrainRegionOriginGraphic();

        }
        private void SetTrainRegionCenterDataChanged(object sender, EventArgs e)
        {
            var region = new CogRectangle();

            var centerX = Convert.ToDouble(this.TrainRegionParam.FirstOrDefault(t => t.Name.Equals("CenterX")).Value);
            var centerY = Convert.ToDouble(this.TrainRegionParam.FirstOrDefault(t => t.Name.Equals("CenterY")).Value);
            var width = Convert.ToDouble(this.TrainRegionParam.FirstOrDefault(t => t.Name.Equals("Width")).Value);
            var height = Convert.ToDouble(this.TrainRegionParam.FirstOrDefault(t => t.Name.Equals("Height")).Value);

            region.SetCenterWidthHeight(centerX, centerY, width, height);
            region.GraphicDOFEnable = CogRectangleDOFConstants.Size | CogRectangleDOFConstants.Position;
            region.Interactive = true;

            cogBlobTool.Region = region;
            cogTrainRegion = region;

            TrainOriginParam.Where(x => x.Name.Equals("OriginX")).FirstOrDefault().Value = region.X;
            TrainOriginParam.Where(x => x.Name.Equals("OriginY")).FirstOrDefault().Value = region.Y;
            TrainRegionParam.Where(x => x.Name.Equals("Width")).FirstOrDefault().Value = region.Width;
            TrainRegionParam.Where(x => x.Name.Equals("Height")).FirstOrDefault().Value = region.Height;

            TrainOriginParam.Where(x => x.Name.Equals("Width")).FirstOrDefault().Value = region.Width;
            TrainOriginParam.Where(x => x.Name.Equals("Height")).FirstOrDefault().Value = region.Height;

            GetTrainRegionOriginGraphic();

        }
        private void GetTrainRegionOriginGraphic()
        {
            if (imageSourceSelectedItem == "TraingImage") return;

            CogDisplay.ClearGraphics();

            var graphic = new CogGraphicInteractiveCollection();
            graphic.Add(cogTrainRegion);
            CogDisplay.SetGraphic(graphic, "TrainRegion", true);
        }

        #endregion

        #region Command
        private void ExecuteImageLoadCommand()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"D:\LOG";
            openFileDialog.Filter = "bmp files (*.bmp)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
            }
            var loadImage = lib.LoadImage(filePath);
            inputImage = loadImage;

            if(inputImage != null) 
                this.CogDisplay.SetImage(loadImage);
        }

        private void ExecuteImageSaveCommand()
        {
            if (inputImage == null) return;

            var path = ConfigurationManager.AppSettings["SAVE_PATH"];

            cameraManager.CogJobs[eExecuteZone][eGrabPosition].SaveImage(inputImage, path);
        }
        private void ExcuteAddMorphologyCommand()
        {
            CogBlobMorphologyCollection blobMorphology = cogBlobTool.RunParams.MorphologyOperations;

            var addMorphology = MorphologyModeItem;

            blobMorphology.Add(addMorphology);

            MorphologyParam = new System.Collections.ObjectModel.ObservableCollection<ListItem>();

            foreach (CogBlobMorphologyConstants item in blobMorphology)
            {
                MorphologyParam.Add(new ListItem() { Name = item.ToString() });
            }

            cogBlobTool.RunParams.MorphologyOperations = blobMorphology;
        }
        private void ExcuteDeleteMorphologyCommand()
        {
            CogBlobMorphologyCollection blobMorphology = cogBlobTool.RunParams.MorphologyOperations;

            if (blobMorphology.Count == 0) return;

            blobMorphology.RemoveAt(blobMorphology.Count - 1);

            MorphologyParam = new System.Collections.ObjectModel.ObservableCollection<ListItem>();

            foreach (CogBlobMorphologyConstants item in blobMorphology)
            {
                MorphologyParam.Add(new ListItem() { Name = item.ToString() });
            }

            cogBlobTool.RunParams.MorphologyOperations = blobMorphology;
        }
        private void ExcuteFitInImageCommand()
        {
            var cameraInfo = sql.CameraInfo.GetAll().FirstOrDefault(x => x.CamID.Equals(eCamID.Cam1));

            var region = new CogRectangle();

            region.SetCenterWidthHeight(cameraInfo.Width / 2, cameraInfo.Height / 2, cameraInfo.Width, cameraInfo.Height);
            region.GraphicDOFEnable = CogRectangleDOFConstants.Size | CogRectangleDOFConstants.Position;
            region.Interactive = true;

            cogBlobTool.Region = region;
            cogTrainRegion = region;

            TrainOriginParam.Where(x => x.Name.Equals("OriginX")).FirstOrDefault().Value = region.X;
            TrainOriginParam.Where(x => x.Name.Equals("OriginY")).FirstOrDefault().Value = region.Y;
            TrainOriginParam.Where(x => x.Name.Equals("Width")).FirstOrDefault().Value = region.Width;
            TrainOriginParam.Where(x => x.Name.Equals("Height")).FirstOrDefault().Value = region.Height;

            TrainRegionParam.Where(x => x.Name.Equals("CenterX")).FirstOrDefault().Value = region.CenterX;
            TrainRegionParam.Where(x => x.Name.Equals("CenterY")).FirstOrDefault().Value = region.CenterY;
            TrainRegionParam.Where(x => x.Name.Equals("Width")).FirstOrDefault().Value = region.Width;
            TrainRegionParam.Where(x => x.Name.Equals("Height")).FirstOrDefault().Value = region.Height;

            GetTrainRegionOriginGraphic();
        }
        private void ExcuteGrabCommand()
        {
            this.CogDisplay.ClearImage();
            this.cogDisplay.ClearGraphics();

            if (this.camera.IsGrabbing)
            {
                this.camera.StopGrabContinuous();
                GSG.NET.Concurrent.LockUtils.Wait(500);
            }
            var image = (ICogImage)this.camera.GrabOneShot();
            this.inputImage = image;

            if(inputImage != null)
                this.cogDisplay.SetImage(image);
        }
        private void ExcuteTrainCommand()
        {
            if (inputImage == null) return;

            trainImage = inputImage;

            if (ImageSourceSelectedItem == "TrainImage")
                this.cogDisplay.SetImage(trainImage);
        }
        private async void ExcuteSaveCommand()
        {
            var view = this.provider.Resolve<ComfirmationView>();
            view.ViewModel.Message = "Do you want to save ?";

            var result = await DialogHost.Show(view, "RootDialog") as bool?;
            if (result == true)
            {
                cogPMAlignTool.Pattern.TrainImage = trainImage;

                TrainPMAlignTool(cogPMAlignTool);
                TrainBlobTool(cogBlobTool);

                cameraManager.CogJobs[eExecuteZone][eGrabPosition].SaveTools();
            }
        }
        private void TrainPMAlignTool(CogPMAlignTool tool)
        {
            if (tool == null) return;

            if (tool.Pattern.TrainImage == null) return;

            cameraManager.CogJobs[eExecuteZone][eGrabPosition].PMAlignDic[PatternID] = tool;
        }
        private void TrainBlobTool(CogBlobTool tool)
        {
            if (tool == null) return;

            cameraManager.CogJobs[eExecuteZone][eGrabPosition].BlobDic[BlobID] = tool;
        }

        private void ExcuteRunBlobToolCommand()
        {
            //ResultGraphic = new CogGraphicInteractiveCollection();
            CogDisplay.ClearGraphics();

            var minSize = sql.OverlapInfo.GetAll().FirstOrDefault().MinSize;

            List<BlobResult> ResultList;

            cameraManager.CogJobs[eExecuteZone][eGrabPosition].RunBlobTool(BlobID, inputImage, out ResultList, (int)minSize);

            if (ResultList == null || ResultList.Count == 0) return;

            outputImage = inputImage;

            BlobResultList = ResultList;

            if (imageSourceSelectedItem == "OutputImage")
            {
                for (int i = 0; i < BlobResultList.Count; i++)
                {
                    GetBlobRunResultGraphic(BlobResultList[i].BlobGraphic);
                }
                cogDisplay.SetImage(outputImage);
            }

            var zoneID = eExecuteZone.MLCC_INSPECTION;
            var posID = eGrabPosition.First;
            var camID = eCamID.Cam1;

            var message = new GUIEventArgs();
            message.MessageKind = eGUIMessageKind.RunProcesser;
            message.ProcessPosition.ZoneID = zoneID;
            message.ProcessPosition.GrabPosition = posID;
            message.ProcessPosition.CamID = camID;
            message.LoadImage = inputImage;
            this._gUIMessageEvent.Publish(message);
        }

        #endregion

    }
}
