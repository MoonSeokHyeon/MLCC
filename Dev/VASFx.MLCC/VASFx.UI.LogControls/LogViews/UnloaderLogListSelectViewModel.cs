using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.UI.LogControls.LogViews
{
    public class UnloaderLogListSelectViewModel : BindableBase, IDialogAware
    {
        public string Title => "UnloaderLogListSelectViewModel";

        public event Action<IDialogResult> RequestClose;

        public UnloaderLogListSelectViewModel()
        {

        }

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
