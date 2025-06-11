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

namespace VASFx.UI.EditControlViews.UI
{
    /// <summary>
    /// LoadingDistributionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingDistributionView : UserControl
    {
        public LoadingDistributionViewModel ViewModel => this.DataContext as LoadingDistributionViewModel;

        public LoadingDistributionView()
        {
            InitializeComponent();

            this.Loaded += LoadingDistributionView_Loaded;
        }

        private void LoadingDistributionView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
