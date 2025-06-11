using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.Common.Interface
{
    public interface ICalibration
    {
        bool GetCoordinateData(XY[] pixelData, XYT movingDistance, XY cameraResol, ref CalibrationData calibrationData);
        bool GetRotationData(XY[] pixelData, XYT movingDistance, XY cameraResol, ref CalibrationData calibrationData);
        XY CalculateCenterOfRotation(XY realDel, XYT movingDegree);
        XY TransPixelToReal(XY pixelData, XY cameraResol, ref CalibrationData calibrationData);
        XY TransRealToRobot(XY realData, ref CalibrationData calibrationData);
    }
}
