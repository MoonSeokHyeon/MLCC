using GSG.NET.WPF.Extensions;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace VASFx.MLCC
{
    /// <summary>
    /// Shell.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Shell : Window
    {
        public ShellViewModel ViewModel { get => this.DataContext as ShellViewModel; }

        public Shell()
        {
            InitializeComponent();

            this.gridHead.MouseLeftButtonDown += GridMain_MouseLeftButtonDown;
            this.Loaded += Shell_Loaded;
            this.Closing += Shell_Closing;
        }

        private void Shell_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            // Get PresentationSource
            PresentationSource presentationSource = PresentationSource.FromVisual((Visual)sender);
            // Subscribe to PresentationSource's ContentRendered event
            presentationSource.ContentRendered += Control_ContentRendered;

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            WindowExternal.MaximizeToFirstMonitor(this);
            this.Activate();

            this.ViewModel.View = this;
            this.ViewModel.Init();
        }

        private void Control_ContentRendered(object sender, EventArgs e)
        {
            // Don't forget to unsubscribe from the event
            ((PresentationSource)sender).ContentRendered -= Control_ContentRendered;

            App.splashScreen.AddMessage("Done !");
            GSG.NET.Quartz.TimerUtils.Once(500, () => { App.splashScreen.LoadComplete(); });
        }
    }
}
