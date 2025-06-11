using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace VASFx.UI.EditControlViews.UI
{
    public class LoaderEditViewPanelInspectionModel : BindableBase, IDialogAware
    {

        #region Property

        #region Present

        public bool _isPresent = true;
        public bool IsPresent
        {
            get { return _isPresent; }
            set { SetProperty(ref _isPresent, value); }
        }

        public int _presentNumberValue = 6;
        public int PresentNumberValue
        {
            get { return _presentNumberValue; }
            set { SetProperty(ref _presentNumberValue, value); }
        }
        public string _FindTypeSelectedName = "Histogram";
        private ComboBoxItem _findTypeSelected;
        public ComboBoxItem FindTypeSelected
        {
            get{ return _findTypeSelected; }
            set { _findTypeSelected = value; OnPropertyChanged("FindTypeSelected");}
        }

        public string _PresentRegionSelectedName = "Region1";
        public ComboBoxItem _presentRegionSelected;
        public ComboBoxItem PresentRegionSelected
        {
            get { return _presentRegionSelected; }
            set { _presentRegionSelected = value; OnPropertyChanged("PresentRegionSelected"); }
        }

        public double _presentMeanValue = 10.5;
        public double PresentMeanValue
        {
            get { return _presentMeanValue; }
            set { SetProperty(ref _presentMeanValue, value); }
        }

        public double _presentCountValue = 5000.2;
        public double PresentCountValue
        {
            get { return _presentCountValue; }
            set { SetProperty(ref _presentCountValue, value); }
        }

        public int _presentFirstValue = 100;
        public int PresentFirstValue
        {
            get { return _presentFirstValue; }
            set { SetProperty(ref _presentFirstValue, value); }
        }

        public int _presentLastValue = 255;
        public int PresentLastValue
        {
            get { return _presentLastValue; }
            set { SetProperty(ref _presentLastValue, value); }
        }

        public bool _isPanelHist = true;
        public bool IsPanelHist
        {
            get { return _isPanelHist; }
            set { SetProperty(ref _isPanelHist, value); }
        }

        public bool _isResultGraphic = true;
        public bool IsResultGraphic
        {
            get { return _isResultGraphic; }
            set { SetProperty(ref _isResultGraphic, value); }
        }

        public bool _isReturnResults = false;
        public bool IsReturnResults
        {
            get { return _isReturnResults; }
            set { SetProperty(ref _isReturnResults, value); }
        }
        #endregion

        #region OverPocket
        public bool _isOverPocket = true;
        public bool IsOverPocket
        {
            get { return _isOverPocket; }
            set { SetProperty(ref _isOverPocket, value); }
        }

        public string _PatmaxSelectedName = "Use one pattern for one region";
        private ComboBoxItem _patmaxSelected;
        public ComboBoxItem PatmaxSelected
        {
            get { return _patmaxSelected; }
            set { _patmaxSelected = value; OnPropertyChanged("PatmaxSelected"); }
        }

        public string _ShiftSelectedName = "Find Patmax";
        private ComboBoxItem _shiftSelected;
        public ComboBoxItem ShiftSelected
        {
            get { return _shiftSelected; }
            set { _shiftSelected = value; OnPropertyChanged("ShiftSelected"); }
        }

        public string _RegionSelectedName = "Region1";
        private ComboBoxItem _regionSelected;
        public ComboBoxItem RegionSelected
        {
            get { return _regionSelected; }
            set { _regionSelected = value; OnPropertyChanged("RegionSelected"); }
        }

        public double _pocketContrastValue = 10.0;
        public double PocketContrastValue
        {
            get { return _pocketContrastValue; }
            set { SetProperty(ref _pocketContrastValue, value); }
        }

        public double _pocketAngleValue = 30.5;
        public double PocketAngleValue
        {
            get { return _pocketAngleValue; }
            set { SetProperty(ref _pocketAngleValue, value); }
        }

        public double _pocketScoreValue = 40.5;
        public double PocketScoreValue
        {
            get { return _pocketScoreValue; }
            set { SetProperty(ref _pocketScoreValue, value); }
        }

        public int _pocketGraphicValue = 0;
        public int PocketGraphicValue
        {
            get { return _pocketGraphicValue; }
            set { SetProperty(ref _pocketGraphicValue, value); }
        }
        #endregion

        #region Over Pocket Params

        public string _PanelRegionSelectedName = "Region1";
        private ComboBoxItem _panelRegionSelected;
        public ComboBoxItem PanelRegionSelected
        {
            get { return _panelRegionSelected; }
            set { _panelRegionSelected = value; OnPropertyChanged("PanelRegionSelected"); }
        }

        public double _pocketDeltaXValue = 10.5;
        public double PocketDeltaXValue
        {
            get { return _pocketDeltaXValue; }
            set { SetProperty(ref _pocketDeltaXValue, value); }
        }

        public double _pocketDeltaYValue = 10.5;
        public double PocketDeltaYValue
        {
            get { return _pocketDeltaYValue; }
            set { SetProperty(ref _pocketDeltaYValue, value); }
        }

        public double _pocketDeltaTValue = 10.5;
        public double PocketDeltaTValue
        {
            get { return _pocketDeltaTValue; }
            set { SetProperty(ref _pocketDeltaTValue, value); }
        }
        #endregion

        #endregion


        #region ICommand

        #region Present

        public ICommand PresentNumber { get; set; }
        public ICommand PresentSearchRegion { get; set; }
        public ICommand PresentMean { get; set; }
        public ICommand PresentCount { get; set; }
        public ICommand PresentFirst { get; set; }
        public ICommand PresentLast { get; set; }
        #endregion

        #region OverPocket
        public ICommand PocketSearchRegion { get; set; }
        public ICommand PocketTrainRegion { get; set; }
        public ICommand PocketTrain { get; set; }
        public ICommand PocketOrigin { get; set; }
        public ICommand PocketReset { get; set; }
        public ICommand PocketTrainCurrentAll { get; set; }
        public ICommand PocketTrainCurrent { get; set; }
        public ICommand PocketContrast { get; set; }
        public ICommand PocketAngle { get; set; }
        public ICommand PocketScore { get; set; }
        public ICommand PocketGraphic { get; set; }
        #endregion

        #region Over Pocket Params
        public ICommand PocketDeltaX { get; set; }
        public ICommand PocketDeltaY { get; set; }
        public ICommand PocketDeltaT { get; set; }
        #endregion


        #endregion
        public string Title => "LoaderEditViewPanelInspectionModel";

        public event Action<IDialogResult> RequestClose;

        public LoaderEditViewPanelInspectionModel()
        {
            PresentNumber = new DelegateCommand<string>(ExecutePresentNumberCommand);
            PresentSearchRegion = new DelegateCommand(ExecutePresentSearchRegionCommand);
            PresentMean = new DelegateCommand<string>(ExecutePresentMeanCommand);
            PresentCount = new DelegateCommand<string>(ExecutePresentCountCommand);
            PresentFirst = new DelegateCommand<string>(ExecutePresentFirstCommand);
            PresentLast = new DelegateCommand<string>(ExecutePresentLastCommand);

            PocketSearchRegion = new DelegateCommand(ExecutePocketSearchRegionCommand);
            PocketTrainRegion = new DelegateCommand(ExecutePocketTrainRegionCommand);
            PocketTrain = new DelegateCommand(ExecutePocketTrainCommand);
            PocketOrigin = new DelegateCommand(ExecutePocketOriginCommand);
            PocketReset = new DelegateCommand(ExecutePocketResetCommand);
            PocketTrainCurrentAll = new DelegateCommand(ExecutePocketTrainCurrentAllCommand);
            PocketTrainCurrent = new DelegateCommand(ExecutePocketTrainCurrentCommand);
            PocketContrast = new DelegateCommand<string>(ExecutePocketContrastCommand);
            PocketAngle = new DelegateCommand<string>(ExecutePocketAngleCommand);
            PocketScore = new DelegateCommand<string>(ExecutePocketScoreCommand);
            PocketGraphic = new DelegateCommand<string>(ExecutePocketGraphicCommand);

            PocketDeltaX = new DelegateCommand<string>(ExecutePocketDeltaXCommand);
            PocketDeltaY = new DelegateCommand<string>(ExecutePocketDeltaYCommand);
            PocketDeltaT = new DelegateCommand<string>(ExecutePocketDeltaTCommand);

        }

        internal void Init()
        {
            
        }

        private void OnPropertyChanged(string v)
        {
            if (v.Equals("FindTypeSelected"))
            {
                if (FindTypeSelected.Content != null)
                    _FindTypeSelectedName = FindTypeSelected.Content.ToString();
            }
            else if (v.Equals("PresentRegionSelected"))
            {
                if (PresentRegionSelected.Content != null)
                    _PresentRegionSelectedName = PresentRegionSelected.Content.ToString();
            }
            else if (v.Equals("PatmaxSelected"))
            {
                if (PatmaxSelected.Content != null)
                    _PatmaxSelectedName = PatmaxSelected.Content.ToString();
            }
            else if (v.Equals("ShiftSelected"))
            {
                if (ShiftSelected.Content != null)
                    _ShiftSelectedName = ShiftSelected.Content.ToString();
            }
            else if (v.Equals("RegionSelected"))
            {
                if (RegionSelected.Content != null)
                    _RegionSelectedName = RegionSelected.Content.ToString();
            }
            else if (v.Equals("PanelRegionSelected"))
            {
                if (PanelRegionSelected.Content != null)
                    _PanelRegionSelectedName = PanelRegionSelected.Content.ToString();
            }

        }

        #region Present Command

        private void ExecutePresentNumberCommand(string obj)
        {
            if (obj.Equals("Up"))
                PresentNumberValue = PresentNumberValue + 1;
            else
                PresentNumberValue = PresentNumberValue - 1;
        }
        

        private void ExecutePresentSearchRegionCommand()
        {

        }

        private void ExecutePresentMeanCommand(string obj)
        {
            if (obj.Equals("Up"))
                PresentMeanValue = PresentMeanValue + 1;
            else
                PresentMeanValue = PresentMeanValue - 1;
        }

        private void ExecutePresentCountCommand(string obj)
        {
            if (obj.Equals("Up"))
                PresentCountValue = PresentCountValue + 1;
            else
                PresentCountValue = PresentCountValue - 1;
        }

        private void ExecutePresentFirstCommand(string obj)
        {
            if (obj.Equals("Up"))
                PresentFirstValue = PresentFirstValue + 1;
            else
                PresentFirstValue = PresentFirstValue - 1;
        }

        private void ExecutePresentLastCommand(string obj)
        {
            if (obj.Equals("Up"))
                PresentLastValue = PresentLastValue + 1;
            else
                PresentLastValue = PresentLastValue - 1;
        }

        #endregion

        #region OverPocket Command
        private void ExecutePocketSearchRegionCommand()
        {

        }

        private void ExecutePocketTrainRegionCommand()
        {

        }

        private void ExecutePocketTrainCommand()
        {

        }

        private void ExecutePocketOriginCommand()
        {

        }

        private void ExecutePocketResetCommand()
        {

        }

        private void ExecutePocketTrainCurrentAllCommand()
        {

        }

        private void ExecutePocketTrainCurrentCommand()
        {

        }

        private void ExecutePocketContrastCommand(string obj)
        {
            if (obj.Equals("Up"))
                PocketContrastValue = PocketContrastValue + 1;
            else
                PocketContrastValue = PocketContrastValue - 1;
        }

        private void ExecutePocketAngleCommand(string obj)
        {
            if (obj.Equals("Up"))
                PocketAngleValue = PocketAngleValue + 1;
            else
                PocketAngleValue = PocketAngleValue - 1;
        }

        private void ExecutePocketScoreCommand(string obj)
        {
            if (obj.Equals("Up"))
                PocketScoreValue = PocketScoreValue + 1;
            else
                PocketScoreValue = PocketScoreValue - 1;
        }

        private void ExecutePocketGraphicCommand(string obj)
        {
            if (obj.Equals("Up"))
                PocketGraphicValue = PocketGraphicValue + 1;
            else
                PocketGraphicValue = PocketGraphicValue - 1;
        }
        #endregion

        #region Over Pocket Params
        private void ExecutePocketDeltaXCommand(string obj)
        {
            if (obj.Equals("Up"))
                PocketDeltaXValue = PocketDeltaXValue + 1;
            else
                PocketDeltaXValue = PocketDeltaXValue - 1;
        }

        private void ExecutePocketDeltaYCommand(string obj)
        {
            if (obj.Equals("Up"))
                PocketDeltaYValue = PocketDeltaYValue + 1;
            else
                PocketDeltaYValue = PocketDeltaYValue - 1;
        }

        private void ExecutePocketDeltaTCommand(string obj)
        {
            if (obj.Equals("Up"))
                PocketDeltaTValue = PocketDeltaTValue + 1;
            else
                PocketDeltaTValue = PocketDeltaTValue - 1;
        }
        #endregion

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
