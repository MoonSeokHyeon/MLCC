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
    /// CameraOptionControlView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CameraOptionControlView : UserControl
    {
        public CameraOptionControlViewModel ViewModel => this.DataContext as CameraOptionControlViewModel;

        public CameraOptionControlView()
        {
            InitializeComponent();

            this.Loaded += CameraOptionControlView_Loaded;
        }

        private void CameraOptionControlView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
