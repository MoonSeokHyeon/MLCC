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

namespace VASFx.UI.LogControls.LogViews
{
    /// <summary>
    /// LoaderLogListSelectView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UnloaderLogListSelectView : UserControl
    {
        public UnloaderLogListSelectViewModel ViewModel => this.DataContext as UnloaderLogListSelectViewModel;
        public UnloaderLogListSelectView()
        {
            InitializeComponent();
            this.Loaded += UnloaderLogListSelectView_Loaded;
        }

        bool isInited = false;
        private void UnloaderLogListSelectView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInited)
            {
                isInited = true;
                this.ViewModel.Init();
            }
        }
    }
}
