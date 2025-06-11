using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.Shared;

namespace VASFx.UI.EditControlViews.UI
{
    public class ModelMeasureViewModel : BindableBase
    {

        #region Properties

        #endregion


        #region ICommand

        public ICommand CloseDialogCommand { get; set; }

        #endregion

        public eExecuteZone ZoneID { get; set; }

        public ModelMeasureViewModel()
        {
            InitICommand();
        }

        private void InitICommand()
        {
            this.CloseDialogCommand = new DelegateCommand(ExcuteCloseDialogCommand);
        }

        private void ExcuteCloseDialogCommand()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        public void Init()
        {
        }
    }
}
