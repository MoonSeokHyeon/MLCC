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

namespace VASFx.UI.OptionControl.UI
{
    /// <summary>
    /// OverlapOptionControlView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OverlapOptionControlView : UserControl
    {
        public OverlapOptionControlViewModel ViewModel => this.DataContext as OverlapOptionControlViewModel;

        public OverlapOptionControlView()
        {
            InitializeComponent();
            this.Loaded += OverlapOptionControlView_Loaded;
        }
        private void OverlapOptionControlView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
