using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.UI.OptionControl.UI;

namespace VASFx.UI.OptionControl
{
    public class OptionViewsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            ViewModelLocationProvider.Register<CameraOptionControlView, CameraOptionControlViewModel>();
            ViewModelLocationProvider.Register<LightOptionControlView, LightOptionControlViewModel>();
            ViewModelLocationProvider.Register<OverlapOptionControlView, OverlapOptionControlViewModel>();
            ViewModelLocationProvider.Register<SystemOptionView, SystemOptionViewModel>();
            ViewModelLocationProvider.Register<SystemSettingView, SystemSettingViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
