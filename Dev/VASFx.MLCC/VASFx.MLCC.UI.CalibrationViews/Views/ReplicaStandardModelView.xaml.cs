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

namespace VASFx.MLCC.UI.CalibrationViews.Views
{
    /// <summary>
    /// ReplicaStandardModelView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReplicaStandardModelView : UserControl
    {
        public ReplicaStandardModelView()
        {
            InitializeComponent();

            this.Loaded += ReplicaStandardModel_Loaded;
        }

        private void ReplicaStandardModel_Loaded(object sender, RoutedEventArgs e)
        {

            this.DataContext = this;
        }

        public string SectionOne
        {
            get { return (string)GetValue(SectionOneProperty); }
            set { SetValue(SectionOneProperty, value); }
        }

        public static readonly DependencyProperty SectionOneProperty =
            DependencyProperty.Register("SectionOne", typeof(string), typeof(ReplicaStandardModelView), new PropertyMetadata(null));

        public string SectionTwo
        {
            get { return (string)GetValue(SectionTwoProperty); }
            set { SetValue(SectionTwoProperty, value); }
        }

        public static readonly DependencyProperty SectionTwoProperty =
            DependencyProperty.Register("SectionTwo", typeof(string), typeof(ReplicaStandardModelView), new PropertyMetadata(null));

        public string ModelName
        {
            get { return (string)GetValue(ModelNameProperty); }
            set { SetValue(ModelNameProperty, value); }
        }

        public static readonly DependencyProperty ModelNameProperty =
            DependencyProperty.Register("ModelName", typeof(string), typeof(ReplicaStandardModelView), new PropertyMetadata(null));
    }
}
