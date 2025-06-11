using VASFx.UI.CogDisplayViews.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;

namespace VASFx.UI.CogDisplayViews
{
    public class CogDisplayViewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //ViewModelLocationProvider.Register<CogDisplayView, CogDisplayViewModel>();
            ViewModelLocationProvider.Register<CogDisplaySingleView, CogDisplaySingleViewModel>();
            ViewModelLocationProvider.Register<CogDisplayDualView, CogDisplayDualViewModel>();
            ViewModelLocationProvider.Register<CogDisplayQuadView, CogDisplayQuadViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
