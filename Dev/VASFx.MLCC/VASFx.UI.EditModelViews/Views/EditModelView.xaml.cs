using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VASFx.UI.EditModelViews.UI
{
    /// <summary>
    /// EditModelView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditModelView : UserControl
    {
        EditModelViewModel ViewModel => this.DataContext as EditModelViewModel;

        public ICommand ShowEditorClick
        {
            get { return (ICommand)GetValue(ShowEditorClickProperty); }
            set { SetValue(ShowEditorClickProperty, value); }
        }

        public static readonly System.Windows.DependencyProperty ShowEditorClickProperty =
            DependencyProperty.Register("ShowEditorClick", typeof(ICommand), typeof(EditModelView), new UIPropertyMetadata(null));

        public EditModelView()
        {
            InitializeComponent();

            this.Loaded += EditModelView_Loaded;
        }

        private void EditModelView_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }
    }
}
