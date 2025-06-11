using GSG.NET.Extensions;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using VASFx.Common.Events;
using VASFx.Common.Shared;

namespace VASFx.UI.LogControls.LogViews
{
    public class LogListViewModel : BindableBase
    {
        public eExecuteZone Zone { get; set; } = eExecuteZone.NONE;

        private ObservableCollection<CoreEventArgs> logList = new ObservableCollection<CoreEventArgs>();

        public ObservableCollection<CoreEventArgs> LogList
        {
            get { return logList; }
            set { SetProperty(ref this.logList, value); ; }
        }

        public LogListViewModel(IEventAggregator aggregator)
        {
            aggregator.GetEvent<CoreMessageEvent>().Unsubscribe(UICallbackCommunication);
            aggregator.GetEvent<CoreMessageEvent>().Subscribe(UICallbackCommunication, ThreadOption.UIThread);
        }

        private void UICallbackCommunication(CoreEventArgs obj)
        {
            switch (obj.MessageKind)
            {
                case eCoreMessageKind.SystemStateChange:
                    break;
                case eCoreMessageKind.ModelChange:
                    break;
                case eCoreMessageKind.ChangeModelList:
                    break;
                case eCoreMessageKind.LightValueChange:
                    break;
                case eCoreMessageKind.Calibration:
                    break;
                case eCoreMessageKind.CalibrationComplete:
                    break;
                case eCoreMessageKind.AddedAlignHistory:
                    break;
                case eCoreMessageKind.AddedInspectionHistory:
                    break;
                case eCoreMessageKind.FailModelChange:
                    break;
                case eCoreMessageKind.CameraPropertyChanged:
                    break;
                case eCoreMessageKind.ResultGraphicChange:
                    break;
                case eCoreMessageKind.ViewLoggerChanged:
                    AddList(obj);
                    break;
                default:
                    break;
            }
        }

        private void AddList(CoreEventArgs obj)
        {
            LogList.Insert(0, obj);

            if (LogList.Count > 200)
                LogList.RemoveAt(LogList.Count - 1);
        }
    }
}
