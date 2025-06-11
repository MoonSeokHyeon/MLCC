using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.UI.EditModelViews.UI;

namespace VASFx.UI.EditModelViews
{
    public class EditModelViewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            ViewModelLocationProvider.Register<EditModelView, EditModelViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
