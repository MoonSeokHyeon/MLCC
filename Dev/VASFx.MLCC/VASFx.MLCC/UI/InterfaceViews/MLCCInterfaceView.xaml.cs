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
using VASFx.UI.InterfaceView;

namespace VASFx.MLCC.UI.InterfaceViews
{
    /// <summary>
    /// MLCCInterfaceView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MLCCInterfaceView : UserControl
    {
        public MLCCInterfaceViewModel ViewModel { get => this.DataContext as MLCCInterfaceViewModel; }

        public MLCCInterfaceView()
        {
            InitializeComponent();
            this.Loaded += PLCInterfaceHTypeViewModel_Loaded;

        }
        private void PLCInterfaceHTypeViewModel_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
