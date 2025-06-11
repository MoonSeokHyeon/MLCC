using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Common.VisionModel
{
    public class PatternDataConfig
    {
        public double AngleLow { get; set; }
        public double AngleHigh { get; set; }

        public double ScaleLow { get; set; }
        public double ScaleHigh { get; set; }

        public double StartX { get; set; }
        public double StartY { get; set; }
        public double LengthX { get; set; }
        public double LengthY { get; set; }

        public double OrignX { get; set; }
        public double OrignY { get; set; }
        public double ScreenXLength { get; set; }
        public double ScreenYLength { get; set; }
        public double OriginRotation { get; set; }
    }
}
