using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.UI.Interactivity
{
    public class InteractivityModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            ViewModelLocationProvider.Register<ComfirmationView, ComfirmationViewModel>();
            ViewModelLocationProvider.Register<NotificationView, NotificationViewModel>();
            ViewModelLocationProvider.Register<InputValueView, InputValueViewModel>();
            ViewModelLocationProvider.Register<ModelManagerView, ModelManagerViewModel>();

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
