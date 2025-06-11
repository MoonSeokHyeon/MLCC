using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Enum;

namespace VASFx.MLCC.Common.VisionModel
{
    public class LightConfig
    {
        public int ID { get; set; }
        public eCamID CamID { get; set; }
        public eLightPointKind LightPointKind { get; set; }
        public string ControllerID { get; set; }
        public int Chanel { get; set; }
    }
}
