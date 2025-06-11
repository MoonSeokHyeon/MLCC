using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VASFx.UI.EditControlViews.UI
{
    public class MeasureSampleViewModel : BindableBase, IDialogAware
    {
        public string Title => "MeasureSampleView";

        public double widthValue = 100.32;
        public double WidthValue
        {
            get { return widthValue; }
            set { SetProperty(ref widthValue, value); }
        }

        public double heightValue = 120.32;
        public double HeightValue
        {
            get { return heightValue; }
            set { SetProperty(ref heightValue, value); }
        }

        public ICommand GetLength { get; set; }
        public ICommand Save { get; set; }
        public ICommand Close { get; set; }
        public event Action<IDialogResult> RequestClose;


        public MeasureSampleViewModel()
        {
            GetLength = new DelegateCommand(ExecuteGetLengthCommand);
            Save = new DelegateCommand(ExecuteSaveCommand);
            Close = new DelegateCommand(ExecuteCloseCommand);
        }

        private void ExecuteGetLengthCommand()
        {

        }

        private void ExecuteSaveCommand()
        {

        }

        private void ExecuteCloseCommand()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

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
