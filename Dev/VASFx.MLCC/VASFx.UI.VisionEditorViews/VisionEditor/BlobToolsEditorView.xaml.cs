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

namespace VASFx.UI.VisionEditorViews.VisionEditor
{
    /// <summary>
    /// BlobToolsEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BlobToolsEditorView : UserControl
    {
        public BlobToolsEditorViewModel ViewModel => this.DataContext as BlobToolsEditorViewModel;
        public BlobToolsEditorView()
        {
            InitializeComponent();
            this.Loaded += BlobToolsEditorView_Loaded;
        }

        private void BlobToolsEditorView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Init();
        }

    }
}
