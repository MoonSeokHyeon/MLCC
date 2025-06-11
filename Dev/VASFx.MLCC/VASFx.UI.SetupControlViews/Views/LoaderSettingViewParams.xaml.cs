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
    /// LoaderSettingViewParams.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoaderSettingViewParams : UserControl
    {
        public LoaderSettingViewParamsModel ViewModel => this.DataContext as LoaderSettingViewParamsModel;
        public LoaderSettingViewParams()
        {
            InitializeComponent();
            this.Loaded += LoaderSettingViewParams_Loaded;
        }

        bool isInited = false;
        private void LoaderSettingViewParams_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInited)
            {
                isInited = true;
                this.ViewModel.Init();
            }

        }
    }
}
