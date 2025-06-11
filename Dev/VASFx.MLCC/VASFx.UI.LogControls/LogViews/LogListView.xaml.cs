using System.Windows.Controls;
using System.Windows.Media;
using VASFx.Common.Events;
using VASFx.Common.Shared;

namespace VASFx.UI.LogControls.LogViews
{
    /// <summary>
    /// LogListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogListView : UserControl
    {
        public LogListViewModel ViewModel => this.DataContext as LogListViewModel;

        public LogListView()
        {
            InitializeComponent();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var data = e.Row.DataContext as CoreEventArgs;
            switch ( data.SubKind )
            {
                case eViewLogArgsLevelKind.Warn:
                    e.Row.Foreground = Brushes.Brown;
                    break;
                case eViewLogArgsLevelKind.Fail:
                    e.Row.Foreground = Brushes.Red;
                    break;
                case eViewLogArgsLevelKind.Info:
                    e.Row.Foreground = Brushes.Black;
                    break;
                default:
                    break;
            }
        }
    }
}
