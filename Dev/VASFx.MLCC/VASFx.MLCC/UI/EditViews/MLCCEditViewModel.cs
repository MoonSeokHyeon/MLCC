using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.UI.VisionEditorViews.VisionEditor;

namespace VASFx.MLCC.UI.EditViews
{
    public class MLCCEditViewModel : BindableBase
    {
        BlobToolsEditorView mlccEditorView = null;
        public BlobToolsEditorView MlccEditorView { get => this.mlccEditorView; set => SetProperty(ref this.mlccEditorView, value); }

        IContainerProvider provider = null;
        public MLCCEditViewModel(IContainerProvider prov)
        {
            provider = prov;

            this.MlccEditorView = provider.Resolve<BlobToolsEditorView>();

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
