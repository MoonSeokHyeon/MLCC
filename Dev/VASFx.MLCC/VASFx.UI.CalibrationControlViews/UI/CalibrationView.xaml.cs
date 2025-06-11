using GSG.NET.Extensions;
using GSG.NET.Utils;
using GSG.NET.WPF.Extensions;
using GSG.NET.WPF.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using VASFx.Common.Delegate;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.UI.CalibrationControlViews.UI
{
    /// <summary>
    /// CalibrationView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CalibrationView : Window, IWindowView
    {
        public IWindowViewModel ViewModel => this.DataContext as IWindowViewModel;

        public Window OwnerWindow { get; private set; }

        public CalibrationView()
        {
            InitializeComponent();

            this.gridHeader.MouseLeftButtonDown += GridMain_MouseLeftButtonDown;
            this.Loaded += PatternRegistView_Loaded;
        }

        private void GridMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.ClickCount > 1)
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    WindowExternal.MaximizeToFirstMonitor(this);
            }
            else
                this.DragMove();
        }


        public event WindowShowEventHandler WindowShow;
        public event WindowCloseEventHandler WindowClose;

        private void PatternRegistView_Loaded(object sender, RoutedEventArgs e)
        {
            // Get PresentationSource
            PresentationSource presentationSource = PresentationSource.FromVisual((Visual)sender);
            // Subscribe to PresentationSource's ContentRendered event
            presentationSource.ContentRendered += Control_ContentRendered;

            this.ViewModel.View = this;
        }

        private void Control_ContentRendered(object sender, EventArgs e)
        {
            // Don't forget to unsubscribe from the event
            ((PresentationSource)sender).ContentRendered -= Control_ContentRendered;

            this.ViewModel.Init();
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
            WindowClose?.Invoke();
        }

        new public void Show()
        {
            Assert.NotNull(this.ViewModel, $"{this.GetType().Name} - View Model is Null");

            if (this.OwnedWindows != null)
                base.Owner = this.OwnerWindow;
            else
                base.Owner = this.OwnerWindow = Application.Current.MainWindow;

            WindowHelper.SetPopupToOwnerCenterPoint(base.Owner, this);
            ViewModel.Subscribe();
            ViewModel.Query();

            this.RaiseWindowShow();
            base.ShowDialog();
        }

        public void ShowDialog(object zoneID)
        {
            Assert.NotNull(this.ViewModel, $"{this.GetType().Name} - View Model is Null");

            if (this.OwnerWindow != null)
                base.Owner = this.OwnerWindow;
            else
                base.Owner = this.OwnerWindow = Application.Current.MainWindow;

            var viewModel = this.ViewModel as CalibrationViewModel;
            viewModel.ZoneID = CastTo<eExecuteZone>.From<object>(zoneID);
            viewModel.SetUserControlViewModelCamID();

            //WindowHelper.SetPopupToOwnerCenterPoint(base.Owner, this);
            ViewModel.Subscribe();
            ViewModel.Query();

            this.RaiseWindowShow();
            base.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Focus();

            Assert.NotNull(this.ViewModel, $"{this.GetType().Name} - View Model is Null");

            this.ViewModel.Unsubscribe();
            this.ViewModel.Clear();

            base.OnClosing(e);
        }
    }
}