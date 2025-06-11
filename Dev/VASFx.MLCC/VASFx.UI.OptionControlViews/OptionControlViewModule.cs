using VASFx.UI.OptionControlViews.UI;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionControlViews
{
    public class OptionControlViewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            ViewModelLocationProvider.Register<OptionControlView, OptionControlViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
