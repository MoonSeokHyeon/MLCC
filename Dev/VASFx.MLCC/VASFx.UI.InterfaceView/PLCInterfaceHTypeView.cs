using System.Windows;
using System.Windows.Controls;

namespace VASFx.UI.InterfaceView
{
    /// <summary>
    /// PLCInterfaceView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PLCInterfaceHTypeView : UserControl
    {
        public PLCInterfaceHTypeViewModel ViewModel => this.DataContext as PLCInterfaceHTypeViewModel;

        public PLCInterfaceHTypeView()
        {
            InitializeComponent();

            this.Loaded += PLCInterfaceHTypeView_Loaded;
        }

        private void PLCInterfaceHTypeView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Init();
        }
    }
}
