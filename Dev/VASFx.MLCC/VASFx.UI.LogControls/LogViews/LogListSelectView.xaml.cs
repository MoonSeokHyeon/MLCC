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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VASFx.UI.LogControls.LogViews
{
    /// <summary>
    /// LogListSelectView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogListSelectView : UserControl
    {
        LogListSelectViewModel ViewModel => this.DataContext as LogListSelectViewModel;

        public LogListSelectView()
        {
            InitializeComponent();

            this.Loaded += LogListSelectView_Loaded;
        }

        private void LogListSelectView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
