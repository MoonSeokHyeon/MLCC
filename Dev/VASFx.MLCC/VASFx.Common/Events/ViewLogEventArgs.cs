using System;
using VASFx.Common.Shared;

namespace VASFx.Common.Events
{
    public class ViewLogEventArgs : EventArgs
    {
        public eViewLogArgsKind Kind { get; set; }
        public eViewLogArgsLevelKind Level { get; set; }
        public eExecuteZone Zone { get; set; }
        public string Message { get; set; }
        public DateTime CreateTime { get; set; }

        public ViewLogEventArgs()
        {
            this.CreateTime = DateTime.Now;
            this.Kind = eViewLogArgsKind.PLC;
            this.Level = eViewLogArgsLevelKind.Info;
            this.Message = string.Empty;
        }
    }
}
