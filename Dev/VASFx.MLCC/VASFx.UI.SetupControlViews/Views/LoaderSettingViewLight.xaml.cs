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

namespace VASFx.UI.SetupControlViews.Views
{
    /// <summary>
    /// LoaderSettingViewLight.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoaderSettingViewLight : UserControl
    {
        public LoaderSettingViewLightModel ViewModel => this.DataContext as LoaderSettingViewLightModel;
        public LoaderSettingViewLight()
        {
            InitializeComponent();
            this.Loaded += LoaderSettingViewLight_Loaded;
        }

        bool isInited = false;
        private void LoaderSettingViewLight_Loaded(object sender, RoutedEventArgs e)
        {       
            if (!isInited)
            {
                ViewModel.Chanel1Value = Chanel1Value;
                ViewModel.Chanel2Value = Chanel2Value;
                isInited = true;
                this.ViewModel.Init();
            }
        }
    }
}
