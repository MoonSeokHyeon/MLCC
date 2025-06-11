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
    /// InspectionLogView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectionLogView : UserControl
    {
        public InspectionLogViewModel ViewModel { get => this.DataContext as InspectionLogViewModel; }

        public InspectionLogView()
        {
            InitializeComponent();
            this.Loaded += InspectionLogView_Loaded;
        }

        private void InspectionLogView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Init();
        }
    }
}
