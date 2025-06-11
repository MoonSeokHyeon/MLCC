using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.Shared;
using VASFx.UI.VisionEditorViews;

namespace VASFx.UI.CalibrationControlViews.UI
{
    public class CalibrationConfigViewModel : BindableBase
    {
        #region Properties

        private double coordinateXMovingValue;
        public double CoordinateXMovingValue
        {
            get { return coordinateXMovingValue; }
            set { SetProperty(ref this.coordinateXMovingValue, value); }
        }

        private double coordinateYMovingValue;
        public double CoordinateYMovingValue
        {
            get { return coordinateYMovingValue; }
            set { SetProperty(ref this.coordinateYMovingValue, value); }
        }

        private double rotateTMovingValue;
        public double RotateTMovingValue
        {
            get { return rotateTMovingValue; }
            set { SetProperty(ref this.rotateTMovingValue, value); }
        }

        private bool isSaving;
        public bool IsSaving { get => this.isSaving; set => SetProperty(ref this.isSaving, value); }

        #endregion

        #region ICommands

        public ICommand SaveCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        #endregion

        public eExecuteZone ZoneID { get; set; }

        public CalibrationConfigViewModel()
        {
            InitICommands();
        }

        private void InitICommands()
        {
            this.SaveCommand = new DelegateCommand(ExcuteSaveCommand);
            this.CloseCommand = new DelegateCommand(ExcuteCloseDialogCommand);
        }

        private async void ExcuteSaveCommand()
        {
            this.IsSaving = true;

            await Task.Delay(TimeSpan.FromSeconds(1));

            this.IsSaving = false;

            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void ExcuteCloseDialogCommand()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        public void Init()
        {
            this.CoordinateXMovingValue = 0.00;
            this.CoordinateYMovingValue = 0.00;
            this.RotateTMovingValue = 0.00;
        }
    }
}
