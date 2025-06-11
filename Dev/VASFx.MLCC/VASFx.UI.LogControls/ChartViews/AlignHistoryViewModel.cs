using GSG.NET.Vision.Model;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Events;
using Prism.Mvvm;
using System.Linq;
using System.Windows.Media;
using VASFx.Common.HistoryModel;
using VASFx.Common.Shared;
using VASFx.MLCC.Sqlite;

namespace VASFx.UI.LogControls.ChartViews
{
    public class AlignHistoryViewModel : BindableBase
    {
        private XYT _resultXYT = new XYT();
        public XYT ResultXYT { get => _resultXYT; set => SetProperty(ref _resultXYT, value); }
        
        private XYT _resultAveXYT = new XYT();
        public XYT ResultAvrXYT { get => _resultAveXYT; set => SetProperty(ref _resultAveXYT, value); }

        SeriesCollection _lastAlignSeries;
        public SeriesCollection LastAlignSeries { get => _lastAlignSeries; set => SetProperty(ref _lastAlignSeries, value); }

        public eExecuteZone Zone { get; set; }

        SqlManager sql = null;

        public AlignHistoryViewModel(IEventAggregator eventAggregator, SqlManager sql)
        {
            this.sql = sql;

            this.LastAlignSeries = new SeriesCollection();
            this.LastAlignSeries.Add(new LineSeries { Title = "X", Name = "X", Values = new ChartValues<ObservableValue>(), Fill = Brushes.Transparent });
            this.LastAlignSeries.Add(new LineSeries { Title = "Y", Name = "Y", Values = new ChartValues<ObservableValue>(), Fill = Brushes.Transparent });
            this.LastAlignSeries.Add(new LineSeries { Title = "T", Name = "T", Values = new ChartValues<ObservableValue>(), Fill = Brushes.Transparent });
        }

        bool isInited = false;
        internal void Init()
        {
            if (isInited) return;
            isInited = true;

            var hisData = sql.AlignHistory.GetAll().Where(x => x.Zone == this.Zone && x.IsSuccess == true).OrderByDescending(x => x.CreateDate).Take(30).ToList();
            if (hisData == null) return;

            hisData.ForEach(x =>
            {
                this.LastAlignSeries[0].Values.Add(new ObservableValue { Value = x.Result.X });
                this.LastAlignSeries[1].Values.Add(new ObservableValue { Value = x.Result.Y });
                this.LastAlignSeries[2].Values.Add(new ObservableValue { Value = x.Result.T });
            });
        }

        void AddChartData(object data)
        {
            var log = GSG.NET.Extensions.CastTo<AlignHistory>.From(data);
            if (!log.IsSuccess) return;

            if (this.Zone != log.Zone) return;

            var averResult = new XYT();
            this.LastAlignSeries.Cast<LineSeries>().ToList().ForEach(x =>
            {
                if (x.Values.Count > 30)
                    x.Values.RemoveAt(0);

                var aver = (x.Values.Cast<ObservableValue>().Select(i => i.Value).ToArray()).Average();
                switch (x.Name)
                {
                    case "X":
                        averResult.X = aver;
                        break;
                    case "Y":
                        averResult.Y = aver;
                        break;
                    case "T":
                        averResult.T = aver;
                        break;
                    default:
                        break;
                }
            });
            ResultAvrXYT = averResult;

            this.LastAlignSeries[0].Values.Add(new ObservableValue { Value = log.Result.X });
            this.LastAlignSeries[1].Values.Add(new ObservableValue { Value = log.Result.Y });
            this.LastAlignSeries[2].Values.Add(new ObservableValue { Value = log.Result.T });

            var currentXYT = new XYT(log.Result.X, log.Result.Y, log.Result.T);
            this.ResultXYT = currentXYT;
        }

    }
}
