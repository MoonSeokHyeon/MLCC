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
using VASFx.MLCC.UI.VisionSettingViews.Views;

namespace VASFx.MLCC.UI.CalibrationViews.Views
{
    /// <summary>
    /// CalSelectTwoSectionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CalSelectTwoSectionView : UserControl
    {
        public CalSelectTwoSectionView()
        {
            InitializeComponent();
            this.Loaded += CalibrationSelectTwoSection_Loaded;

        }
        private void CalibrationSelectTwoSection_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        public ICommand SelectCalibration
        {
            get { return (ICommand)GetValue(SelectionProperty); }
            set { SetValue(SelectionProperty, value); }
        }

        public static readonly DependencyProperty SelectionProperty =
            DependencyProperty.Register("SelectionCalibration", typeof(ICommand), typeof(ReplicaStandardModelView), new UIPropertyMetadata(null));

        public ICommand PatternClick
        {
            get { return (ICommand)GetValue(PatternClickProperty); }
            set { SetValue(PatternClickProperty, value); }
        }

        public static readonly DependencyProperty PatternClickProperty =
            DependencyProperty.Register("PatternClick", typeof(ICommand), typeof(CalSelectTwoSectionView), new UIPropertyMetadata(null));

        public ICommand LineClick
        {
            get { return (ICommand)GetValue(LineClickProperty); }
            set { SetValue(LineClickProperty, value); }
        }

        public static readonly DependencyProperty LineClickProperty =
            DependencyProperty.Register("LineClick", typeof(ICommand), typeof(CalSelectTwoSectionView), new UIPropertyMetadata(null));
    }
}
