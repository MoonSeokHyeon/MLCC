using System.Windows;
using System.Windows.Controls;

namespace VASFx.UI.MLCCControls
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MeshResultControl : UserControl
    {
        public string MeshID
        { 
            get { return (string)GetValue(MeshIDProperty );}
            set { SetValue(MeshIDProperty , value); }
        }
        public static readonly DependencyProperty MeshIDProperty =
            DependencyProperty.Register("MeshID", typeof(string), typeof(MeshResultControl), new PropertyMetadata(string.Empty));

        public Visibility ResultOKVisibility
        { 
            get { return (Visibility)GetValue(ResultOKVisibilityProperty); }
            set { SetValue(ResultOKVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ResultOKVisibilityProperty =
            DependencyProperty.Register("ResultOKVisibility", typeof(Visibility), typeof(MeshResultControl), new PropertyMetadata(Visibility.Collapsed));

        public MeshResultControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }       
    }
}
