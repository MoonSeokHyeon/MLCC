using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.UI.EditControlViews.UI;

namespace VASFx.UI.EditControlViews
{
    public class EditControlViewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            ViewModelLocationProvider.Register<ModelMeasureView, ModelMeasureViewModel>();
            ViewModelLocationProvider.Register<LoadingDistributionView, LoadingDistributionViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
