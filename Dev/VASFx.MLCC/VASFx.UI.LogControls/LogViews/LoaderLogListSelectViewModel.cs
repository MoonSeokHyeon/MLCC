﻿using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.UI.LogControls.LogViews
{
    public class LoaderLogListSelectViewModel : BindableBase, IDialogAware
    {

        public string Title => "UnloaderLogListSelectViewModel";

        public event Action<IDialogResult> RequestClose;

        public LoaderLogListSelectViewModel()
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
