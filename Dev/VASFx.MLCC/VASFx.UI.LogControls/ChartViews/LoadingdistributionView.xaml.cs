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

namespace VASFx.UI.LogControls.ChartViews
{
    /// <summary>
    /// LoadingdistributionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingdistributionView : UserControl
    {
        public LoadingdistributionViewModel ViewModel => this.DataContext as LoadingdistributionViewModel;
        public LoadingdistributionView()
        {
            InitializeComponent();

            this.Loaded += LoadingdistributionView_Loaded;
        }

        private void LoadingdistributionView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
