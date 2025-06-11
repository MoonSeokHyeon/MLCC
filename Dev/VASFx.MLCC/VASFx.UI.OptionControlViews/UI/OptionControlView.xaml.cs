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

namespace VASFx.UI.OptionControlViews.UI
{
    /// <summary>
    /// OptionControlView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OptionControlView : UserControl
    {
        OptionControlViewModel ViewModel => this.DataContext as OptionControlViewModel;

        public OptionControlView()
        {
            InitializeComponent();

            this.Loaded += OptionControlView_Loaded;
        }

        private void OptionControlView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
