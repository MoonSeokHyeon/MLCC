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

namespace VASFx.UI.LightControlViews.UI
{
    /// <summary>
    /// SettingLightView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingLightView : UserControl
    {
        public SettingLightViewModel ViewModel { get => this.DataContext as SettingLightViewModel; }

        public SettingLightView()
        {
            InitializeComponent();
            this.Loaded += SettingLightView_Loaded;
        }
        private void SettingLightView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            this.ViewModel.ExecuteSetLightValueCommand();
        }
    }
}
