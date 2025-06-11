using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Shared;

namespace VASFx.Device.LightController
{
    public class LightValueConfig
    {
        public int Id { get; set; }
        public int Channel { get; set; }
        public eExecuteZone ZoneID { get; set; }
        public eGrabPosition GrabPos { get; set; }
        public int LightValue { get; set; }
    }
}
