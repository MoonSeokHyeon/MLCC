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
    public class LoaderEditViewTrayInspectionModel : BindableBase, IDialogAware
    {
        #region Property

        #region Tray Pattern
        public double _patternContrastValue = 18.2;
        public double PatternContrastValue
        {
            get { return _patternContrastValue; }
            set { SetProperty(ref _patternContrastValue, value); }
        }

        public double _patternAngleValue = 22.5;
        public double PatternAngleValue
        {
            get { return _patternAngleValue; }
            set { SetProperty(ref _patternAngleValue, value); }
        }

        public double _patternScoreValue = 50.2;
        public double PatternScoreValue
        {
            get { return _patternScoreValue; }
            set { SetProperty(ref _patternScoreValue, value); }
        }

        public int _patternGraphicValue = 3;
        public int PatternGraphicValue
        {
            get { return _patternGraphicValue; }
            set { SetProperty(ref _patternGraphicValue, value); }
        }
        #endregion

        #region Tray Align
        public int _alignPocketValue = 8;
        public int AlignPocketValue
        {
            get { return _alignPocketValue; }
            set { SetProperty(ref _alignPocketValue, value); }
        }

        public double _alignContrastValue = 15.2;
        public double AlignContrastValue
        {
            get { return _alignContrastValue; }
            set { SetProperty(ref _alignContrastValue, value); }
        }

        public double _alignAngleValue = 20.5;
        public double AlignAngleValue
        {
            get { return _alignAngleValue; }
            set { SetProperty(ref _alignAngleValue, value); }
        }

        public double _alignScoreValue = 60.5;
        public double AlignScoreValue
        {
            get { return _alignScoreValue; }
            set { SetProperty(ref _alignScoreValue, value); }
        }

        public int _alignGraphicValue = 5;
        public int AlignGraphicValue
        {
            get { return _alignGraphicValue; }
            set { SetProperty(ref _alignGraphicValue, value); }
        }

        public string _RegionSelectedName = "Region1";
        private ComboBoxItem _regionSelected;
        public ComboBoxItem RegionSelected
        {
            get
            {
                return _regionSelected;
            }
            set
            {
                _regionSelected = value;
                OnPropertyChanged("RegionSelected");
            }
        }

        public bool _isOneRegion = true;
        public bool IsOneRegion
        {
            get { return _isOneRegion; }
            set { SetProperty(ref _isOneRegion, value); }
        }

        #endregion

        #region Pocket Params
        public string _PocketNumSelectedName = "Pocket 1";
        private ComboBoxItem _pocketNumSelected;
        public ComboBoxItem PocketNumSelected
        {
            get
            {
                return _pocketNumSelected;
            }
            set
            {
                _pocketNumSelected = value;
                OnPropertyChanged("PocketNumSelected");
            }
        }

        public bool _isLimitation = true;
        public bool IsLimitation
        {
            get { return _isLimitation; }
            set { SetProperty(ref _isLimitation, value); }
        }

        public double _limitMaxXValue = 100.5;
        public double LimitMaxXValue
        {
            get { return _limitMaxXValue; }
            set { SetProperty(ref _limitMaxXValue, value); }
        }

        public double _limitMaxYValue = 100.5;
        public double LimitMaxYValue
        {
            get { return _limitMaxYValue; }
            set { SetProperty(ref _limitMaxYValue, value); }
        }

        public double _limitMaxTValue = 100.5;
        public double LimitMaxTValue
        {
            get { return _limitMaxTValue; }
            set { SetProperty(ref _limitMaxTValue, value); }
        }

        public double _limitMinXValue = -100.5;
        public double LimitMinXValue
        {
            get { return _limitMinXValue; }
            set { SetProperty(ref _limitMinXValue, value); }
        }

        public double _limitMinYValue = -100.5;
        public double LimitMinYValue
        {
            get { return _limitMinYValue; }
            set { SetProperty(ref _limitMinYValue, value); }
        }

        public double _limitMinTValue = -100.5;
        public double LimitMinTValue
        {
            get { return _limitMinTValue; }
            set { SetProperty(ref _limitMinTValue, value); }
        }

        public bool _isOffset = false;
        public bool IsOffset
        {
            get { return _isOffset; }
            set { SetProperty(ref _isOffset, value); }
        }

        public double _offsetXValue = 0.0;
        public double OffsetXValue
        {
            get { return _offsetXValue; }
            set { SetProperty(ref _offsetXValue, value); }
        }

        public double _offsetYValue = 0.0;
        public double OffsetYValue
        {
            get { return _offsetYValue; }
            set { SetProperty(ref _offsetYValue, value); }
        }

        public double _offsetTValue = 0.0;
        public double OffsetTValue
        {
            get { return _offsetTValue; }
            set { SetProperty(ref _offsetTValue, value); }
        }
        #endregion
        #endregion

        #region ICommand

        #region Tray Pattern
        public ICommand PatternSearchRegion { get; set; }
        public ICommand PatternTrainRegion { get; set; }
        public ICommand PatternTrain { get; set; }
        public ICommand PatternOrigin { get; set; }
        public ICommand PatternReset { get; set; }
        public ICommand PatternTrainCurrent { get; set; }
        public ICommand PatternContrast { get; set; }
        public ICommand PatternAngle { get; set; }
        public ICommand PatternScore { get; set; }
        public ICommand PatternGraphic { get; set; }
        #endregion

        #region Tray Align
        public ICommand AlignSearchRegion { get; set; }
        public ICommand AlignTrainRegion { get; set; }
        public ICommand AlignTrain { get; set; }
        public ICommand AlignOrigin { get; set; }
        public ICommand AlignReset { get; set; }
        public ICommand AlignPocket { get; set; }
        public ICommand AlignContrast { get; set; }
        public ICommand AlignAngle { get; set; }
        public ICommand AlignScore { get; set; }
        public ICommand AlignGraphic { get; set; }
        #endregion

        #region Pocket Params
        public ICommand LimitMaxX { get; set; }
        public ICommand LimitMaxY { get; set; }
        public ICommand LimitMaxT { get; set; }
        public ICommand LimitMinX { get; set; }
        public ICommand LimitMinY { get; set; }
        public ICommand LimitMinT { get; set; }
        public ICommand OffsetX { get; set; }
        public ICommand OffsetY { get; set; }
        public ICommand OffsetT { get; set; }
        #endregion

        #endregion
        public string Title => "LoaderEditViewTrayInspectionModel";

        public event Action<IDialogResult> RequestClose;

        public LoaderEditViewTrayInspectionModel()
        {
            PatternSearchRegion = new DelegateCommand(ExecutePatternSearchRegionCommand);
            PatternTrainRegion = new DelegateCommand(ExecutePatternTrainRegionCommand);
            PatternTrain = new DelegateCommand(ExecutePatternTrainCommand);
            PatternOrigin = new DelegateCommand(ExecutePatternOriginCommand);
            PatternReset = new DelegateCommand(ExecutePatternResetCommand);
            PatternTrainCurrent = new DelegateCommand(ExecutePatternTrainCurrentCommand);
            PatternContrast = new DelegateCommand<string>(ExecutePatternContrastCommand);
            PatternAngle = new DelegateCommand<string>(ExecutePatternAngleCommand);
            PatternScore = new DelegateCommand<string>(ExecutePatternScoreCommand);
            PatternGraphic = new DelegateCommand<string>(ExecutePatternGraphicCommand);

            AlignSearchRegion = new DelegateCommand(ExecuteAlignSearchRegionCommand);
            AlignTrainRegion = new DelegateCommand(ExecuteAlignTrainRegionCommand);
            AlignTrain = new DelegateCommand(ExecuteAlignTrainCommand);
            AlignOrigin = new DelegateCommand(ExecuteAlignOriginCommand);
            AlignReset = new DelegateCommand(ExecuteAlignResetCommand);
            AlignPocket = new DelegateCommand<string>(ExecuteAlignPocketCommand);
            AlignContrast = new DelegateCommand<string>(ExecuteAlignContrastCommand);
            AlignAngle = new DelegateCommand<string>(ExecuteAlignAngleCommand);
            AlignScore = new DelegateCommand<string>(ExecuteAlignScoreCommand);
            AlignGraphic = new DelegateCommand<string>(ExecuteAlignGraphicCommand);

            LimitMaxX = new DelegateCommand<string>(ExecuteLimitMaxXCommand);
            LimitMaxY = new DelegateCommand<string>(ExecuteLimitMaxYCommand);
            LimitMaxT = new DelegateCommand<string>(ExecuteLimitMaxTCommand);
            LimitMinX = new DelegateCommand<string>(ExecuteLimitMinXCommand);
            LimitMinY = new DelegateCommand<string>(ExecuteLimitMinYCommand);
            LimitMinT = new DelegateCommand<string>(ExecuteLimitMinTCommand);
            OffsetX = new DelegateCommand<string>(ExecuteOffsetXCommand);
            OffsetY = new DelegateCommand<string>(ExecuteOffsetYCommand);
            OffsetT = new DelegateCommand<string>(ExecuteOffsetTCommand);
        }

        internal void Init()
        {
            
        }

        #region Tray Pattern Command
        private void ExecutePatternSearchRegionCommand()
        {
            
        }

        private void ExecutePatternTrainRegionCommand()
        {

        }

        private void ExecutePatternTrainCommand()
        {

        }

        private void ExecutePatternOriginCommand()
        {

        }

        private void ExecutePatternResetCommand()
        {

        }

        private void ExecutePatternTrainCurrentCommand()
        {

        }

        private void ExecutePatternContrastCommand(string obj)
        {
            if (obj.Equals("Up"))
                PatternContrastValue = PatternContrastValue + 1;
            else
                PatternContrastValue = PatternContrastValue - 1;
        }

        private void ExecutePatternAngleCommand(string obj)
        {
            if (obj.Equals("Up"))
                PatternAngleValue = PatternAngleValue + 1;
            else
                PatternAngleValue = PatternAngleValue - 1;
        }

        private void ExecutePatternScoreCommand(string obj)
        {
            if (obj.Equals("Up"))
                PatternScoreValue = PatternScoreValue + 1;
            else
                PatternScoreValue = PatternScoreValue - 1;
        }

        private void ExecutePatternGraphicCommand(string obj)
        {
            if (obj.Equals("Up"))
                PatternGraphicValue = PatternGraphicValue + 1;
            else
                PatternGraphicValue = PatternGraphicValue - 1;
        }
        #endregion

        #region Tray Align Command
        private void ExecuteAlignSearchRegionCommand()
        {

        }

        private void ExecuteAlignTrainRegionCommand()
        {

        }

        private void ExecuteAlignTrainCommand()
        {

        }

        private void ExecuteAlignOriginCommand()
        {

        }

        private void ExecuteAlignResetCommand()
        {

        }

        private void ExecuteAlignPocketCommand(string obj)
        {
            if (obj.Equals("Up"))
                AlignPocketValue = AlignPocketValue + 1;
            else
                AlignPocketValue = AlignPocketValue - 1;
        }

        private void ExecuteAlignContrastCommand(string obj)
        {
            if (obj.Equals("Up"))
                AlignContrastValue = AlignContrastValue + 1;
            else
                AlignContrastValue = AlignContrastValue - 1;
        }

        private void ExecuteAlignAngleCommand(string obj)
        {
            if (obj.Equals("Up"))
                AlignAngleValue = AlignAngleValue + 1;
            else
                AlignAngleValue = AlignAngleValue - 1;
        }

        private void ExecuteAlignScoreCommand(string obj)
        {
            if (obj.Equals("Up"))
                AlignScoreValue = AlignScoreValue + 1;
            else
                AlignScoreValue = AlignScoreValue - 1;
        }

        private void ExecuteAlignGraphicCommand(string obj)
        {
            if (obj.Equals("Up"))
                AlignGraphicValue = AlignGraphicValue + 1;
            else
                AlignGraphicValue = AlignGraphicValue - 1;
        }


        private void OnPropertyChanged(string v)
        {
            if (v.Equals("RegionSelected"))
            {
                if (RegionSelected.Content != null)
                    _RegionSelectedName = RegionSelected.Content.ToString();
            }else if (v.Equals("PocketNumSelected"))
            {
                if (PocketNumSelected.Content != null)
                    _PocketNumSelectedName = PocketNumSelected.Content.ToString();
            }
            
        }

        #endregion

        #region Pocket Params
        private void ExecuteLimitMaxXCommand(string obj)
        {
            if (obj.Equals("Up"))
                LimitMaxXValue = LimitMaxXValue + 1;
            else
                LimitMaxXValue = LimitMaxXValue - 1;
        }

        private void ExecuteLimitMaxYCommand(string obj)
        {
            if (obj.Equals("Up"))
                LimitMaxYValue = LimitMaxYValue + 1;
            else
                LimitMaxYValue = LimitMaxYValue - 1;
        }

        private void ExecuteLimitMaxTCommand(string obj)
        {
            if (obj.Equals("Up"))
                LimitMaxTValue = LimitMaxTValue + 1;
            else
                LimitMaxTValue = LimitMaxTValue - 1;
        }

        private void ExecuteLimitMinXCommand(string obj)
        {
            if (obj.Equals("Up"))
                LimitMinXValue = LimitMinXValue + 1;
            else
                LimitMinXValue = LimitMinXValue - 1;
        }

        private void ExecuteLimitMinYCommand(string obj)
        {
            if (obj.Equals("Up"))
                LimitMinYValue = LimitMinYValue + 1;
            else
                LimitMinYValue = LimitMinYValue - 1;
        }

        private void ExecuteLimitMinTCommand(string obj)
        {
            if (obj.Equals("Up"))
                LimitMinTValue = LimitMinTValue + 1;
            else
                LimitMinTValue = LimitMinTValue - 1;
        }

        private void ExecuteOffsetXCommand(string obj)
        {
            if (obj.Equals("Up"))
                OffsetXValue = OffsetXValue + 1;
            else
                OffsetXValue = OffsetXValue - 1;
        }

        private void ExecuteOffsetYCommand(string obj)
        {
            if (obj.Equals("Up"))
                OffsetYValue = OffsetYValue + 1;
            else
                OffsetYValue = OffsetYValue - 1;
        }

        private void ExecuteOffsetTCommand(string obj)
        {
            if (obj.Equals("Up"))
                OffsetTValue = OffsetTValue + 1;
            else
                OffsetTValue = OffsetTValue - 1;
        }
        #endregion
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
