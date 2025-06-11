using System.Windows.Controls;

namespace VASFx.UI.CalibrationControlViews.UI
{
    /// <summary>
    /// CalibrationConfigView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CalibrationConfigView : UserControl
    {
        public CalibrationConfigViewModel ViewModel => this.DataContext as CalibrationConfigViewModel;

        public CalibrationConfigView()
        {
            InitializeComponent();

            this.ViewModel.Init();
        }
    }
}
