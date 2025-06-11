using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Enum;

namespace VASFx.MLCC.Common.Events
{
    public class GUIEventArgs : EventArgs
    {
        public eVASFxEventKind Kind { get; set; }
        public string MessageKey { get; set; }
        public string MessageText { get; set; }
        public Dictionary<string, object> Args { get; set; }
        public object Arg { get; set; }
    }

    public class CoreEventArgs : EventArgs
    {
        public eVASFxEventKind Kind { get; set; }
        public string MessageKey { get; set; }
        public string MessageText { get; set; }
        public Dictionary<string, object> Args { get; set; }
        public object Arg { get; set; }
    }

    public class ViewLoggerEventArgs : EventArgs
    {
        public eViewLoggerArgsKind Kind { get; set; }
        public eViewLoggerArgsLevelKind Level { get; set; }
        public string Message { get; set; }
        public DateTime CreateTime { get; set; }

        public ViewLoggerEventArgs()
        {
            this.CreateTime = DateTime.Now;
            this.Kind = eViewLoggerArgsKind.PLC;
            this.Level = eViewLoggerArgsLevelKind.Info;
            this.Message = string.Empty;
        }
    }

    public class CameraGrabEventArgs : EventArgs
    {
        public eCamID CamID { get; set; }
        public eGrabQuarter GrabQuarter { get; set; }
        public object ResultImage { get; set; }
        public bool IsClearGraphic { get; set; }

        public CameraGrabEventArgs()
        {
            this.IsClearGraphic = true;
        }
    }
}

