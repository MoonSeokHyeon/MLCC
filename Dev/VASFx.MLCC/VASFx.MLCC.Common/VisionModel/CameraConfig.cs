using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Enum;

namespace VASFx.MLCC.Common.VisionModel
{
    public class CameraConfig
    {
        public eCamID ID { get; set; }
        public eGrabQuarter Position { get; set; }
        public eExecuteZone Zone { get; set; }
        public eImageRotateKind ImageRotate { get; set; }
        public eFindReferanceKind FindReferanceKind { get; set; }

        public string SerialNo { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Description { get; set; }
    }
}
