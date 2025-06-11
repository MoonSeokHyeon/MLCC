using Cognex.VisionPro;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Shared;
using VASFx.UI.VisionEditorViews.VisionEditor;

namespace VASFx.UI.VisionEditorViews
{
    public class VisionEditorViewModule : IModule
    {
        IContainerProvider provider = null;

        bool isOpenManualFindReferanceView = false;

        public VisionEditorViewModule(IContainerProvider provide)
        {
            this.provider = provide;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //ViewModelLocationProvider.Register<ModelEditorView, ModelEditorViewModel>();
            //ViewModelLocationProvider.Register<PMAlignToolEditorView, PMAlignToolEditorViewModel>();
            //ViewModelLocationProvider.Register<FindLineToolEditorView, FindLineToolEditorViewModel>();
            ViewModelLocationProvider.Register<BlobToolsEditorView, BlobToolsEditorViewModel>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
        public void ShowBlobEditorRegistView(eCamID camID, eExecuteZone zoneID, eGrabPosition posID, bool isCalibration)
        {
            
        }

    }
}
