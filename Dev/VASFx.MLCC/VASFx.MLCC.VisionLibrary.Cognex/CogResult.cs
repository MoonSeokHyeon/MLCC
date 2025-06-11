using System;
using System.Collections.Generic;
using Cognex.VisionPro;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.VisionLibrary.Cognex
{
    public class CogResult
    {
        public CogResult()
        {
            ResultXY = new XY(0, 0);
            ResultScore = 0d;
            CornerAngle = 0d;

            ResultGraphic = new CogGraphicInteractiveCollection();
        }

        public ICogImage Image { get; set; }
        public XY ResultXY { get; set; }
        public double ResultScore { get; set; }
        public double CornerAngle { get; set; }
        public CogGraphicInteractiveCollection ResultGraphic { get; set; }
    }
}
