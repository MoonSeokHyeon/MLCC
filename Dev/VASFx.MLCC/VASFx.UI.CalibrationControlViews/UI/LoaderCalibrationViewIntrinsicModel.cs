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

namespace VASFx.UI.CalibrationControlViews.UI
{
    public class LoaderCalibrationViewIntrinsicModel : BindableBase, IDialogAware
    {
        #region Interinsic Calibration Properties
        public double _xSizeValue = 5.3;
        public double XSizeValue
        {
            get { return _xSizeValue; }
            set { SetProperty(ref _xSizeValue, value); }
        }

        public double _ySizeValue = 2.3;
        public double YSizeValue
        {
            get { return _ySizeValue; }
            set { SetProperty(ref _ySizeValue, value); }
        }

        public double _graphicValue = 3;
        public double GraphicValue
        {
            get { return _graphicValue; }
            set { SetProperty(ref _graphicValue, value); }
        }

        public string _UnitSelectedName = "Milimeters";
        private ComboBoxItem _unitSelected;
        public ComboBoxItem UnitSelected
        {
            get
            {
                return _unitSelected;
            }
            set
            {
                _unitSelected = value;
                OnPropertyChanged("UnitSelected");
            }
        }
        #endregion

        #region Interinsic Calibration ICommand
        public ICommand XSize { get; set; }
        public ICommand YSize { get; set; }
        public ICommand Graphic { get; set; }
        public ICommand Import { get; set; }
        public ICommand Export { get; set; }
        public ICommand CalculatedRegion { get; set; }
        public ICommand MaximizeRegion { get; set; }
        public ICommand ExecuteCalculation { get; set; }
        /*public ICommand Cancel { get; set; }
        public ICommand Save { get; set; }*/
        #endregion


        public string Title => "LoaderCalibrationViewIntrinsicModel";

        public event Action<IDialogResult> RequestClose;

        public LoaderCalibrationViewIntrinsicModel()
        {
            XSize = new DelegateCommand<string>(ExecuteXSizeCommand);
            YSize = new DelegateCommand<string>(ExecuteYSizeCommand);
            Graphic = new DelegateCommand<string>(ExecuteGraphicCommand);
            Import = new DelegateCommand(ExecuteImportCommand);
            Export = new DelegateCommand(ExecuteExportCommand);
            CalculatedRegion = new DelegateCommand(ExecuteCalculatedRegionCommand);
            MaximizeRegion = new DelegateCommand(ExecuteMaximizeRegionCommand);
            ExecuteCalculation = new DelegateCommand(ExecuteExecuteCalculationCommand);
            /*Cancel = new DelegateCommand(ExecuteCancelCommand);
            Save = new DelegateCommand(ExecuteSaveCommand);*/
        }

        #region Intrinsic Calibration Command

        private void OnPropertyChanged(string v)
        {
            if (v.Equals("UnitSelected"))
            {
                if (UnitSelected.Content != null)
                    _UnitSelectedName = UnitSelected.Content.ToString();
            }

        }

        private void ExecuteXSizeCommand(string obj)
        {
            if (obj.Equals("Up"))
                XSizeValue = XSizeValue + 1;
            else
                XSizeValue = XSizeValue - 1;
        }

        private void ExecuteYSizeCommand(string obj)
        {
            if (obj.Equals("Up"))
                YSizeValue = YSizeValue + 1;
            else
                YSizeValue = YSizeValue - 1;
        }

        private void ExecuteGraphicCommand(string obj)
        {
            if (obj.Equals("Up"))
                GraphicValue = GraphicValue + 1;
            else
                GraphicValue = GraphicValue - 1;
        }

        private void ExecuteImportCommand()
        {

        }

        private void ExecuteExportCommand()
        {

        }

        private void ExecuteCalculatedRegionCommand()
        {

        }

        private void ExecuteMaximizeRegionCommand()
        {

        }

        private void ExecuteExecuteCalculationCommand()
        {

        }

        /*private void ExecuteCancelCommand()
        {

        }

        private void ExecuteSaveCommand()
        {

        }*/

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
