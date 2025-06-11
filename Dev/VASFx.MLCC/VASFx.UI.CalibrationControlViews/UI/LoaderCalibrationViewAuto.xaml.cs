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

namespace VASFx.UI.CalibrationControlViews.UI
{
    /// <summary>
    /// LoaderCalibrationViewAuto.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoaderCalibrationViewAuto : UserControl
    {
        public LoaderCalibrationViewAutoModel ViewModel => this.DataContext as LoaderCalibrationViewAutoModel;
        public LoaderCalibrationViewAuto()
        {
            InitializeComponent();
            this.Loaded += LoaderCalibrationViewAuto_Loaded;
        }

        bool isInited = false;
        private void LoaderCalibrationViewAuto_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInited)
            {
                isInited = true;
                this.ViewModel.Init();
            }
        }
    }
}
