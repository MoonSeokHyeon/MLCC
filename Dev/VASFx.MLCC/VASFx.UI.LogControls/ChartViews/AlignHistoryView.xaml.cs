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

namespace VASFx.UI.LogControls.ChartViews
{
    /// <summary>
    /// AlignHistoryView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlignHistoryView : UserControl
    {
        public AlignHistoryViewModel ViewModel => this.DataContext as AlignHistoryViewModel;

        public AlignHistoryView()
        {
            InitializeComponent();

            this.Loaded += AlignHistoryView_Loaded;
        }

        private void AlignHistoryView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
