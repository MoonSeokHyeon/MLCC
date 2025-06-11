using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Interface;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.Algorithm
{
    public class GAlgorithm : IAlgorithm
    {
        public XYT Run2PointAlign(XY[] realPos, XYT offset, ref CalibrationData[] calData)
        {
            var returnXYT = new XYT();

            return returnXYT;
        }

        public XYT Run4PointAlign(XY[] realPos, XYT offset, ref CalibrationData[] calData)
        {
            var returnXYT = new XYT();

            return returnXYT;
        }
    }
}
