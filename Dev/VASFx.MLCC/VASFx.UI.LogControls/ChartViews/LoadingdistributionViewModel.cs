using GSG.NET.Vision.Model;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.HistoryModel;
using VASFx.Common.Shared;
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.LogControls.ChartViews
{
    public class LoadingdistributionViewModel : BindableBase
    {
        public ChartValues<ObservablePoint> DataList { get; set; }
        public ChartValues<ObservablePoint> DistributionList { get; set; }

        public SqlManager sql { get; set; }

        public eExecuteZone ZoneID { get; set; }

        public LoadingdistributionViewModel(SqlManager sql, IEventAggregator eventAggregator)
        {
            this.sql = sql;

            this.DataList = new ChartValues<ObservablePoint>();
            this.DistributionList = new ChartValues<ObservablePoint>();

        }

        bool isInited = false;
        public void Init()
        {
            if (isInited) return;
            this.isInited = true;

            //var hisData = sql.AlignHistory.GetAll().Where(x => x.Zone == this.ZoneID && x.ModelKind == this.ModelKindID && x.IsSuccess == true).OrderByDescending(x => x.CreateDate).Take(30).ToList();

            //var xArray = this.DataList.Select(x => x.X).ToArray();
            //var yArray = this.DataList.Select(x => x.Y).ToArray();
            //var xDistribution = GSG.NET.Utils.NumUtils.Average(xArray);
            //var yDistribution = GSG.NET.Utils.NumUtils.Average(yArray);

            //this.DistributionList.Add(new ObservablePoint(xDistribution, yDistribution));
        }

        void AddChart(object data)
        {
            var log = GSG.NET.Extensions.CastTo<AlignHistory>.From<object>(data);
            if (!log.IsSuccess) return;

            if (this.ZoneID != log.Zone) return;

            var chartData = new ObservablePoint(log.Result.X, log.Result.Y);

            if (this.DataList.Count > 30)
                this.DataList.RemoveAt(0);

            this.DataList.Add(chartData);

            var xArray = this.DataList.Select(x => x.X).ToArray();
            var yArray = this.DataList.Select(x => x.Y).ToArray();
            var xDistribution = GSG.NET.Utils.NumUtils.Average(xArray);
            var yDistribution = GSG.NET.Utils.NumUtils.Average(yArray);

            this.DistributionList.Clear();
            this.DistributionList.Add(new ObservablePoint(xDistribution, yDistribution));
        }
    }
}
