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
    /// LoaderSettingViewFocus.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoaderSettingViewFocus : UserControl
    {
        public LoaderSettingViewFocusModel ViewModel => this.DataContext as LoaderSettingViewFocusModel;
        public LoaderSettingViewFocus()
        {
            InitializeComponent();
            this.Loaded += LoaderSettingViewFocus_Loaded;
        }

        bool isInited = false;
        private void LoaderSettingViewFocus_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInited)
            {
                isInited = true;
                this.ViewModel.Init();
            }

        }
    }
}
