using System.Windows;
using System.Windows.Controls;

namespace VASFx.UI.MLCCControls
{
    /// <summary>
    /// MLCCResultTray.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MLCCResultTray : UserControl
    {
        #region Properties

        public Visibility Mesh1Result
        { 
            get { return (Visibility)GetValue(Mesh1ResultVisibilityProperty ); }
            set { SetValue(Mesh1ResultVisibilityProperty , value); }
        }
        public static readonly DependencyProperty Mesh1ResultVisibilityProperty =
            DependencyProperty.Register("Mesh1Result", typeof(Visibility), typeof(MLCCResultTray) , new PropertyMetadata(Visibility.Visible));

        public Visibility Mesh2Result 
        { 
            get { return (Visibility)GetValue(Mesh2ResultVisibilityProperty ); }
            set { SetValue(Mesh2ResultVisibilityProperty , value); }
        }
        public static readonly DependencyProperty Mesh2ResultVisibilityProperty =
            DependencyProperty.Register("Mesh2Result", typeof(Visibility), typeof(MLCCResultTray), new PropertyMetadata(Visibility.Visible));

        public Visibility Mesh3Result 
        { 
            get { return (Visibility)GetValue(Mesh3ResultVisibilityProperty ); }
            set { SetValue(Mesh3ResultVisibilityProperty , value); }
        }
        public static readonly DependencyProperty Mesh3ResultVisibilityProperty =
            DependencyProperty.Register("Mesh3Result", typeof(Visibility), typeof(MLCCResultTray), new PropertyMetadata(Visibility.Visible));

        public Visibility Mesh4Result 
        { 
            get { return (Visibility)GetValue(Mesh4ResultVisibilityProperty ); }
            set { SetValue(Mesh4ResultVisibilityProperty , value); }
        }
        public static readonly DependencyProperty Mesh4ResultVisibilityProperty =
            DependencyProperty.Register("Mesh4Result", typeof(Visibility), typeof(MLCCResultTray), new PropertyMetadata(Visibility.Visible));

        public Visibility Mesh5Result 
        { 
            get { return (Visibility)GetValue(Mesh5ResultVisibilityProperty ); }
            set { SetValue(Mesh5ResultVisibilityProperty , value); }
        }
        public static readonly DependencyProperty Mesh5ResultVisibilityProperty =
            DependencyProperty.Register("Mesh5Result", typeof(Visibility), typeof(MLCCResultTray), new PropertyMetadata(Visibility.Visible));

        public Visibility Mesh6Result 
        { 
            get { return (Visibility)GetValue(Mesh6ResultVisibilityProperty ); }
            set { SetValue(Mesh6ResultVisibilityProperty , value); }
        }
        public static readonly DependencyProperty Mesh6ResultVisibilityProperty =
            DependencyProperty.Register("Mesh6Result", typeof(Visibility), typeof(MLCCResultTray), new PropertyMetadata(Visibility.Visible));

        #endregion

        public MLCCResultTray()
        {
            InitializeComponent();
        }
    }
}
