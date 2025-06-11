using Prism.Commands;
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
    public class LoaderCalibrationViewAutoModel : BindableBase, IDialogAware
    {
        #region Properties

        #region Auto Calibration Setting Properties
        public bool _isGraphic = true;
        public bool IsGraphic
        {
            get { return _isGraphic; }
            set { SetProperty(ref _isGraphic, value); }
        }

        public bool _isFeatureInfo = false;
        public bool IsFeatureInfo
        {
            get { return _isFeatureInfo; }
            set { SetProperty(ref _isFeatureInfo, value); }
        }

        public double _contrastValue = 15.0;
        public double ContrastValue
        {
            get { return _contrastValue; }
            set { SetProperty(ref _contrastValue, value); }
        }

        public double _angleValue = 20.0;
        public double AngleValue
        {
            get { return _angleValue; }
            set { SetProperty(ref _angleValue, value); }
        }

        public double _scoreValue = 60.0;
        public double ScoreValue
        {
            get { return _scoreValue; }
            set { SetProperty(ref _scoreValue, value); }
        }

        public double _scaleValue = 0.0;
        public double ScaleValue
        {
            get { return _scaleValue; }
            set { SetProperty(ref _scaleValue, value); }
        }
        #endregion

        #region Auto Calibration Translation Properties
        public double _stepsXValue = 5;
        public double StepsXValue
        {
            get { return _stepsXValue; }
            set { SetProperty(ref _stepsXValue, value); }
        }

        public double _stepsYValue = 5;
        public double StepsYValue
        {
            get { return _stepsYValue; }
            set { SetProperty(ref _stepsYValue, value); }
        }

        public bool _isAutoStep = true;
        public bool IsAutoStep
        {
            get { return _isAutoStep; }
            set { SetProperty(ref _isAutoStep, value); }
        }

        public double _rangeXMinValue = 2;
        public double RangeXMinValue
        {
            get { return _rangeXMinValue; }
            set { SetProperty(ref _rangeXMinValue, value); }
        }

        public double _rangeXMaxValue = 2;
        public double RangeXMaxValue
        {
            get { return _rangeXMaxValue; }
            set { SetProperty(ref _rangeXMaxValue, value); }
        }

        public double _rangeYMinValue = 2;
        public double RangeYMinValue
        {
            get { return _rangeYMinValue; }
            set { SetProperty(ref _rangeYMinValue, value); }
        }

        public double _rangeYMaxValue = 2;
        public double RangeYMaxValue
        {
            get { return _rangeYMaxValue; }
            set { SetProperty(ref _rangeYMaxValue, value); }
        }
        #endregion

        #region Auto Calibration Rotation Properties
        public double _angleMinValue = -15;
        public double AngleMinValue
        {
            get { return _angleMinValue; }
            set { SetProperty(ref _angleMinValue, value); }
        }

        public double _angleMaxValue = 15;
        public double AngleMaxValue
        {
            get { return _angleMaxValue; }
            set { SetProperty(ref _angleMaxValue, value); }
        }

        public double _stepsRotatedValue = 3;
        public double StepsRotatedValue
        {
            get { return _stepsRotatedValue; }
            set { SetProperty(ref _stepsRotatedValue, value); }
        }

        public bool _isCantileverCompensation = true;
        public bool IsCantileverCompensation
        {
            get { return _isCantileverCompensation; }
            set { SetProperty(ref _isCantileverCompensation, value); }
        }
        #endregion

        #endregion Properties

        #region ICommand

        #region Auto Calibration Setting Tab ICommand
        public ICommand TrainRegion { get; set; }
        public ICommand Train { get; set; }
        public ICommand SearchRegion { get; set; }
        public ICommand Contrast { get; set; }
        public ICommand Angle { get; set; }
        public ICommand Score { get; set; }
        public ICommand Scale { get; set; }
        #endregion

        #region Auto Calibration Translation Tab ICommand
        public ICommand StepsX { get; set; }
        public ICommand StepsY { get; set; }
        public ICommand RangeXMin { get; set; }
        public ICommand RangeXMax { get; set; }
        public ICommand RangeYMin { get; set; }
        public ICommand RangeYMax { get; set; }
        #endregion

        #region Auto Calibration Rotation ICommand
        public ICommand AngleMin { get; set; }
        public ICommand AngleMax { get; set; }
        public ICommand StepsRotated { get; set; }
        #endregion

        #endregion

        public string Title => "LoaderCalibrationViewAutoModel";

        public event Action<IDialogResult> RequestClose;

        public LoaderCalibrationViewAutoModel()
        {
            TrainRegion = new DelegateCommand(ExecuteTrainRegionCommand);
            Train = new DelegateCommand(ExecuteTrainCommand);
            SearchRegion = new DelegateCommand(ExecuteSearchRegionCommand);
            Contrast = new DelegateCommand<string>(ExecuteContrastCommand);
            Angle = new DelegateCommand<string>(ExecuteAngleCommand);
            Score = new DelegateCommand<string>(ExecuteScoreCommand);
            Scale = new DelegateCommand<string>(ExecuteScaleCommand);

            StepsX = new DelegateCommand<string>(ExecuteStepsXCommand);
            StepsY = new DelegateCommand<string>(ExecuteStepsYCommand);
            RangeXMin = new DelegateCommand<string>(ExecuteRangeXMinCommand);
            RangeXMax = new DelegateCommand<string>(ExecuteRangeXMaxCommand);
            RangeYMin = new DelegateCommand<string>(ExecuteRangeYMinCommand);
            RangeYMax = new DelegateCommand<string>(ExecuteRangeYMaxCommand);

            AngleMin = new DelegateCommand<string>(ExecuteAngleMinCommand);
            AngleMax = new DelegateCommand<string>(ExecuteAngleMaxCommand);
            StepsRotated = new DelegateCommand<string>(ExecuteStepsRotatedCommand);
        }

        #region Setting Command
        private void ExecuteTrainRegionCommand()
        {

        }

        private void ExecuteTrainCommand()
        {

        }

        private void ExecuteSearchRegionCommand()
        {

        }

        private void ExecuteContrastCommand(string obj)
        {
            if (obj.Equals("Up"))
                ContrastValue = ContrastValue + 1;
            else
                ContrastValue = ContrastValue - 1;
        }

        private void ExecuteAngleCommand(string obj)
        {
            if (obj.Equals("Up"))
                AngleValue = AngleValue + 1;
            else
                AngleValue = AngleValue - 1;
        }

        private void ExecuteScoreCommand(string obj)
        {
            if (obj.Equals("Up"))
                ScoreValue = ScoreValue + 1;
            else
                ScoreValue = ScoreValue - 1;
        }

        private void ExecuteScaleCommand(string obj)
        {
            if (obj.Equals("Up"))
                ScaleValue = ScaleValue + 1;
            else
                ScaleValue = ScaleValue - 1;
        }

        #endregion

        #region Translation Command
        private void ExecuteStepsXCommand(string obj)
        {
            if (obj.Equals("Up"))
                StepsXValue = StepsXValue + 1;
            else
                StepsXValue = StepsXValue - 1;
        }

        private void ExecuteStepsYCommand(string obj)
        {
            if (obj.Equals("Up"))
                StepsYValue = StepsYValue + 1;
            else
                StepsYValue = StepsYValue - 1;
        }

        private void ExecuteRangeXMinCommand(string obj)
        {
            if (obj.Equals("Up"))
                RangeXMinValue = RangeXMinValue + 1;
            else
                RangeXMinValue = RangeXMinValue - 1;
        }

        private void ExecuteRangeXMaxCommand(string obj)
        {
            if (obj.Equals("Up"))
                RangeXMaxValue = RangeXMaxValue + 1;
            else
                RangeXMaxValue = RangeXMaxValue - 1;
        }

        private void ExecuteRangeYMinCommand(string obj)
        {
            if (obj.Equals("Up"))
                RangeYMinValue = RangeYMinValue + 1;
            else
                RangeYMinValue = RangeYMinValue - 1;
        }

        private void ExecuteRangeYMaxCommand(string obj)
        {
            if (obj.Equals("Up"))
                RangeYMaxValue = RangeYMaxValue + 1;
            else
                RangeYMaxValue = RangeYMaxValue - 1;
        }

        #endregion

        #region Rotation Command
        private void ExecuteAngleMinCommand(string obj)
        {
            if (obj.Equals("Up"))
                AngleMinValue = AngleMinValue + 1;
            else
                AngleMinValue = AngleMinValue - 1;
        }

        private void ExecuteAngleMaxCommand(string obj)
        {
            if (obj.Equals("Up"))
                AngleMaxValue = AngleMaxValue + 1;
            else
                AngleMaxValue = AngleMaxValue - 1;
        }

        private void ExecuteStepsRotatedCommand(string obj)
        {
            if (obj.Equals("Up"))
                StepsRotatedValue = StepsRotatedValue + 1;
            else
                StepsRotatedValue = StepsRotatedValue - 1;
        }

        #endregion


        internal void Init()
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
    }
}
