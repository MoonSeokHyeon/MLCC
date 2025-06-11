using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VASFx.MLCC.Common.Delegate;

namespace VASFx.MLCC.Common.Interface
{
    public interface IWindowView
    {
        event WindowShowEventHandler WindowShow;
        event WindowCloseEventHandler WindowClose;

        IWindowViewModel ViewModel { get; }

        Window OwnerWindow { get; }
        void WindowStateChange(WindowState state);
        void Close();

        void RaiseWindowShow();
        void RaiseWindowClose();
    }
}
