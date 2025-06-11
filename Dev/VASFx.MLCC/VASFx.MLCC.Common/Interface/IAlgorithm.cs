using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.Common.Interface
{
    public interface IAlgorithm
    {
        XYT Run2PointAlign(XY[] realPos, XYT offset, ref CalibrationData[] calData);
        XYT Run4PointAlign(XY[] realPos, XYT offset, ref CalibrationData[] calData);

    }
}
