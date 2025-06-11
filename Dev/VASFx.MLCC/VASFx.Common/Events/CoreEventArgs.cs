using System;
using VASFx.Common.Model;
using VASFx.Common.Shared;

namespace VASFx.Common.Events
{
    public class CoreEventArgs : EventArgs
    {
        public eCoreMessageKind MessageKind { get; set; }
        public ProcessPosition processPosition { get; set; } = new ProcessPosition();
        public Enum SubKind { get; set; }
        public string MessageHeader { get; set; }
        public string MessageKey { get; set; }
        public string MessageText { get; set; }
        public object Arg { get; set; }
        public DateTime CreateTime { get; set; }

        public CoreEventArgs()
        {
            this.processPosition = new ProcessPosition();
            this.CreateTime = DateTime.Now;

        }
    }
}
