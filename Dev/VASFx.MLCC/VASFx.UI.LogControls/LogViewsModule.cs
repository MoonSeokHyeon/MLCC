using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.UI.LogControls.ChartViews;
using VASFx.UI.LogControls.LogViews;

namespace VASFx.UI.LogControls
{
    public class LogViewsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //Chart
            ViewModelLocationProvider.Register<AlignHistoryView, AlignHistoryViewModel>();
            ViewModelLocationProvider.Register<LoadingdistributionView, LoadingdistributionViewModel>();

            //List
            ViewModelLocationProvider.Register<LogListView, LogListViewModel>();
            ViewModelLocationProvider.Register<InspectionLogView, InspectionLogViewModel>();

            //Image
            ViewModelLocationProvider.Register<LogImageView, LogImageViewModel>();

            //LogSelect
            ViewModelLocationProvider.Register<LogListSelectView, LogListSelectViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
