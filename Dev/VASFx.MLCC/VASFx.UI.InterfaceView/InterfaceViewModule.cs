using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;

namespace VASFx.UI.InterfaceView
{
    public class InterfaceViewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            ViewModelLocationProvider.Register<PLCInterfaceHTypeView, PLCInterfaceHTypeViewModel>();
            ViewModelLocationProvider.Register<PLCInterfaceVTypeView, PLCInterfaceVTypeViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
