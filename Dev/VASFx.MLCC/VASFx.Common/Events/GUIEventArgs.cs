using System;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using Cognex.VisionPro;

namespace VASFx.Common.Events
{
    public class GUIEventArgs : EventArgs
    {
        public eGUIMessageKind MessageKind { get; set; }
        public ProcessPosition ProcessPosition { get; set; } = new ProcessPosition();
        public ICogImage LoadImage { get; set; }
        public Enum SubKind { get; set; }
        public string MessageKey { get; set; }
        public string MessageText { get; set; }
        public object Arg { get; set; }

        public GUIEventArgs()
        {
            this.ProcessPosition = new ProcessPosition();
        }
    }

}
