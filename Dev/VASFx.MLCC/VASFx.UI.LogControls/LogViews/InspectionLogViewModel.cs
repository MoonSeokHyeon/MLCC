using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.HistoryModel;
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.LogControls.LogViews
{
    public class InspectionLogViewModel : BindableBase
    {
        private List<InspectionHistory> logList;
        public List<InspectionHistory> LogList
        {
            get { return this.logList; }
            set { SetProperty(ref this.logList, value); }
        }

        private DateTime selectedTime;
        public DateTime SelectedTime
        {
            get { return selectedTime; }
            set { SetProperty(ref this.selectedTime, value); }
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { SetProperty(ref this.selectedDate, value); }
        }

        public ICommand SearchLogCommand { get; set; }

        SqlManager sql = null;

        public InspectionLogViewModel(SqlManager sqlManager)
        {
            this.sql = sqlManager;
            this.SearchLogCommand = new DelegateCommand(ExecuteSearchLogCommand);
        }
        public void Init()
        {
            this.SelectedDate = DateTime.Now;
            this.SelectedTime = DateTime.Now;

            this.LogList = this.sql.InspectionHistory.GetAll().OrderByDescending(x => x.CreateDate).Take(200).ToList();
        }

        private void ExecuteSearchLogCommand()
        {
            var targetTime = new DateTime(this.SelectedDate.Year, this.SelectedDate.Month, this.SelectedDate.Day, SelectedTime.Hour, 0, 0);
            this.LogList = this.sql.InspectionHistory.GetAll().Where(d => Math.Abs((d.CreateDate - targetTime).TotalHours) < 1).OrderByDescending(x => x.CreateDate).ToList();
        }
    }
}
