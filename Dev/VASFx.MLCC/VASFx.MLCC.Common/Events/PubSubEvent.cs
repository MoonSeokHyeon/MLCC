using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Common.Events
{
    public class ApplicationExitEvent : PubSubEvent<string> { }

    public class ViewLoggerPubSubEvent : PubSubEvent<ViewLoggerEventArgs> { }

    public class CoreMessagePubSubEvent : PubSubEvent<CoreEventArgs> { }

    public class GUIMessagePubSubEvent : PubSubEvent<GUIEventArgs> { }

    public class GrabCamPubSubEvent : PubSubEvent<CameraGrabEventArgs> { }

    public class DBTableChangePubSubEvent : PubSubEvent<string> { }
}
