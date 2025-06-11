using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.Common.Events
{
    public class ViewLogPubSubEvent : PubSubEvent<ViewLogEventArgs> { }
    public class ApplicationExitEvent : PubSubEvent<string> { }
    public class GUIMessageEvent : PubSubEvent<GUIEventArgs> { }
    public class CoreMessageEvent : PubSubEvent<CoreEventArgs> { }
    public class CamDisplayUpdateEvent : PubSubEvent<string> { }
    public class WinformHostFixAirSpace : PubSubEvent<bool> { }
}
