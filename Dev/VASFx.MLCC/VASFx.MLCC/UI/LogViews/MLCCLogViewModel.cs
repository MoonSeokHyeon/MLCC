using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VASFx.Common.HistoryModel;
using VASFx.MLCC.Sqlite;
using VASFx.UI.LogControls.LogViews;

namespace VASFx.MLCC.UI.LogViews
{
    public class MLCCLogViewModel: BindableBase
    {
        InspectionLogView inspectionLogView = null;
        public InspectionLogView InspectionLogView { get => this.inspectionLogView; set => SetProperty(ref this.inspectionLogView, value); }

        IContainerProvider provider = null;

        public MLCCLogViewModel(IContainerProvider prov)
        {
            provider = prov;

            this.InspectionLogView = provider.Resolve<InspectionLogView>();
        }
        public void Init()
        {
        }
    }
}
