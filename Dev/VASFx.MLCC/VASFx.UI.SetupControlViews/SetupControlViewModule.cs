using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.UI.SetupControlViews.Views;

namespace VASFx.UI.SetupControlViews
{
    public class SetupControlViewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            /*ViewModelLocationProvider.Register<LoaderSettingViewPopup, LoaderSettingViewPopupModel>();*/
            ViewModelLocationProvider.Register<LoaderSettingViewParams, LoaderSettingViewParamsModel>();
            ViewModelLocationProvider.Register<LoaderSettingViewLight, LoaderSettingViewLightModel>();
            ViewModelLocationProvider.Register<LoaderSettingViewFocus, LoaderSettingViewFocusModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
