using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Interface;

namespace VASFx.MLCC.Common.VisionModel
{
    public class CaliperDataConfig
    {
        public int Polarity { get; set; }

        public int CaliperNum { get; set; }
        public double SearchLength { get; set; }
        public double ProjectLength { get; set; }
        public double SearchDirection { get; set; }
        public int NumIgnore { get; set; }
        public double ContrastThreshold { get; set; }
        public double PixelsSize { get; set; }

        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }
    }
}
