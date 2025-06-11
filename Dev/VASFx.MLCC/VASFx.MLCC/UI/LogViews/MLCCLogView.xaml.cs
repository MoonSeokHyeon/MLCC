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

namespace VASFx.MLCC.UI.LogViews
{
    /// <summary>
    /// MLCCLogView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MLCCLogView : UserControl
    {
        public MLCCLogViewModel ViewModel => this.DataContext as MLCCLogViewModel;

        public MLCCLogView()
        {
            InitializeComponent();
            this.Loaded += MLCCLogView_Loaded;
        }

        private void MLCCLogView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Init();
        }
    }
}
