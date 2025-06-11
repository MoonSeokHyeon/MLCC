using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class GrabResult
    {
        public eCamID CamID { get; set; }
        public int ImageWidth { get; set; } = 0;
        public int ImageHeight { get; set; } = 0;
        public byte[] PixelData { get; set; }
    }
}
