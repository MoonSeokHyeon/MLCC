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

namespace VASFx.MLCC.UI.OptionViews
{
    /// <summary>
    /// MLCCOptionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MLCCOptionView : UserControl
    {
        public MLCCOptionViewModel ViewModel => this.DataContext as MLCCOptionViewModel;

        public MLCCOptionView()
        {
            InitializeComponent();

            this.Loaded += MLCCOptionView_Loaded;
        }

        private void MLCCOptionView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Init();
        }
    }
}
