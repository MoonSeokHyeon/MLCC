using GSG.NET.Utils;
using GSG.NET.WPF.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VASFx.Common.Delegate;
using VASFx.Common.Interface;

namespace VASFx.UI.EditControlViews.UI
{
    /// <summary>
    /// LoaderEditViewInspection.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditSingleView : Window, IWindowView
    {
        public EditSingleViewModel ViewModel => this.DataContext as EditSingleViewModel;

        IWindowViewModel IWindowView.ViewModel => this.DataContext as IWindowViewModel;

        public Window OwnerWindow { get; private set; }
        delegate void FHideWindow();
        bool? private_dialog_result;

        public EditSingleView()
        {
            InitializeComponent();
            this.Loaded += EditSingleView_Loaded;
        }

        bool isInited = false;

        public event WindowShowEventHandler WindowShow;
        public event WindowCloseEventHandler WindowClose;

        private void EditSingleView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInited)
            {
                isInited = true;
                this.ViewModel.Init();
            }
            this.ViewModel.View = this;
        }

        public new void ShowDialog()
        {
            Assert.NotNull(this.ViewModel, $"{this.GetType().Name} - View Model is Null");

            base.Owner = this.OwnerWindow = Application.Current.MainWindow;

            WindowHelper.SetPopupToOwnerCenterPoint(base.Owner, this);
            ViewModel.Subscribe();
            ViewModel.Query();

            this.RaiseWindowShow();
            base.ShowDialog();
        }

        public void WindowStateChange(WindowState state)
        {
            throw new NotImplementedException();
        }

        public void RaiseWindowShow()
        {
            WindowShow?.Invoke();
        }

        public void RaiseWindowClose()
        {
            WindowShow?.Invoke();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            private_dialog_result = DialogResult;
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new
            FHideWindow(_HideThisWindow));
        }

        void _HideThisWindow()
        {
            this.Hide();
            (typeof(Window)).GetField("_isClosing", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, false);
            (typeof(Window)).GetField("_dialogResult", BindingFlags.Instance |
            BindingFlags.NonPublic).SetValue(this, private_dialog_result);
            private_dialog_result = null;
        }
       
    }
}
