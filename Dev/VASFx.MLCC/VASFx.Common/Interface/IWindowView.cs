using System.Windows;
using VASFx.Common.Delegate;

namespace VASFx.Common.Interface
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
