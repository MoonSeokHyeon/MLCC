using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Shared;
using VASFx.UI.CalibrationControlViews.UI;

namespace VASFx.UI.CalibrationControlViews
{
    public class CalibrationControlViewModule : IModule
    {
        IContainerProvider provider = null;

        public CalibrationControlViewModule(IContainerProvider prov)
        {
            provider = prov;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            ViewModelLocationProvider.Register<CalibrationView, CalibrationViewModel>();
            ViewModelLocationProvider.Register<CalibrationConfigView, CalibrationConfigViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        public void ShowCalibrationRegistView(eExecuteZone zone)
        {
            var view = this.provider.Resolve<CalibrationView>();
            view.ShowDialog(zone);
        }

    }
}
