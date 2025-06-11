using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Shared;
using VASFx.MLCC.UI.EditViews;
using VASFx.MLCC.UI.ImageLogViews;
using VASFx.MLCC.UI.InterfaceViews;
using VASFx.MLCC.UI.LogViews;
using VASFx.MLCC.UI.MainViews;
using VASFx.MLCC.UI.OptionViews;
using VASFx.MLCC.UI.SettingViews;

namespace VASFx.MLCC.UI
{
    public class ShellViewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            ViewModelLocationProvider.Register<MLCCMainView, MLCCMainViewModel>();
            ViewModelLocationProvider.Register<MLCCSettingView, MLCCSettingViewModel>();
            ViewModelLocationProvider.Register<MLCCEditView, MLCCEditViewModel>();
            ViewModelLocationProvider.Register<MLCCOptionView, MLCCOptionViewModel>();
            ViewModelLocationProvider.Register<MLCCLogView, MLCCLogViewModel>();
            ViewModelLocationProvider.Register<MLCCMainImageLogView, MLCCMainImageLogViewModel>();
            ViewModelLocationProvider.Register<MLCCInterfaceView, MLCCInterfaceViewModel>();

            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.MainView, typeof(MLCCMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainView, typeof(MLCCSettingView));
            regionManager.RegisterViewWithRegion(RegionNames.MainView, typeof(MLCCEditView));
            regionManager.RegisterViewWithRegion(RegionNames.MainView, typeof(MLCCOptionView));
            regionManager.RegisterViewWithRegion(RegionNames.MainView, typeof(MLCCLogView));
            regionManager.RegisterViewWithRegion(RegionNames.MainView, typeof(MLCCMainImageLogView));
            regionManager.RegisterViewWithRegion(RegionNames.MainView, typeof(MLCCInterfaceView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
