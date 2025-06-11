using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.UI.InterfaceView;

namespace VASFx.MLCC.UI.InterfaceViews
{
    public class MLCCInterfaceViewModel:BindableBase
    {
        PLCInterfaceHTypeView plcInterfaceHTypeView = null;
        public PLCInterfaceHTypeView PLCInterfaceHTypeView { get => this.plcInterfaceHTypeView; set => SetProperty(ref this.plcInterfaceHTypeView, value); }

        IContainerProvider provider = null;

        public MLCCInterfaceViewModel(IContainerProvider prov)
        {
            provider = prov;

            this.PLCInterfaceHTypeView = provider.Resolve<PLCInterfaceHTypeView>();
            this.PLCInterfaceHTypeView.ViewModel.SubText = "";

            InitICommands();
        }
        private void InitICommands()
        {


        }
        public void Init()
        {
        }

    }
}
