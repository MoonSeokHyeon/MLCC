using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class ProcessPosition
    {
        public eCamID CamID { get; set; }
        public eExecuteZone ZoneID { get; set; }
        public eGrabPosition GrabPosition { get; set; }
    }
}
