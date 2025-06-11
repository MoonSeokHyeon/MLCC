using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VASFx.UI.SetupControlViews.Views
{
    public class LoaderSettingViewParamsModel : BindableBase
    {

        #region Properties

        public double _exposureValue = 10.0;
        public double ExposureValue
        {
            get { return _exposureValue; }
            set { SetProperty(ref _exposureValue, value); }
        }

        public double _gammaValue = 50.5;
        public double GammaValue
        {
            get { return _gammaValue; }
            set { SetProperty(ref _gammaValue, value); }
        }

        public int _gainValue = 10;
        public int GainValue
        {
            get { return _gainValue; }
            set { SetProperty(ref _gainValue, value); }
        }

        public int _digitalShiftValue = 20;

        public event Action<IDialogResult> RequestClose;

        public int DigitalShiftValue
        {
            get { return _digitalShiftValue; }
            set { SetProperty(ref _digitalShiftValue, value); }
        }

        #endregion


        #region ICommand

        public ICommand Exposure { get; set; }
        public ICommand Gamma { get; set; }
        public ICommand Gain { get; set; }
        public ICommand DigitalShift { get; set; }

        #endregion

        public string Title => "LoaderSettingViewParamsModel";

        
        public LoaderSettingViewParamsModel()
        {
            Exposure = new DelegateCommand<string>(ExecuteExposureCommand);
            Gamma = new DelegateCommand<string>(ExecuteGammaCommand);
            Gain = new DelegateCommand<string>(ExecuteGainCommand);
            DigitalShift = new DelegateCommand<string>(ExecuteDigitalShiftCommand);
        }
        internal void Init()
        {
           
        }

        private void ExecuteExposureCommand(string obj)
        {
            if (obj.Equals("Up"))
                ExposureValue = ExposureValue + 1;
            else
                ExposureValue = ExposureValue - 1;
        }

        private void ExecuteGammaCommand(string obj)
        {
            if (obj.Equals("Up"))
                GammaValue = GammaValue + 1;
            else
                GammaValue = GammaValue - 1;
        }

        private void ExecuteGainCommand(string obj)
        {
            if (obj.Equals("Up"))
                GainValue = GainValue + 1;
            else
                GainValue = GainValue - 1;
        }

        private void ExecuteDigitalShiftCommand(string obj)
        {
            if (obj.Equals("Up"))
                DigitalShiftValue = DigitalShiftValue + 1;
            else
                DigitalShiftValue = DigitalShiftValue - 1;
        }
    }
}
