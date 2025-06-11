using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Interface;
using VASFx.MLCC.Common.VisionModel;

namespace VASFx.MLCC.Calibration
{
    public class GCalibration : ICalibration
    {
        public XY CalculateCenterOfRotation(XY realDel, XYT movingDegree)
        {
            var returnXY = new XY();

            return returnXY;
        }

        public bool GetCoordinateData(XY[] pixelData, XYT movingDistance, XY cameraResol, ref CalibrationData calibrationData)
        {
            return true;
        }

        public bool GetRotationData(XY[] pixelData, XYT movingDistance, XY cameraResol, ref CalibrationData calibrationData)
        {
            return true;
        }

        public XY TransPixelToReal(XY pixelData, XY cameraResol, ref CalibrationData calibrationData)
        {
            var returnXY = new XY();

            return returnXY;
        }

        public XY TransRealToRobot(XY realData, ref CalibrationData calibrationData)
        {
            var returnXY = new XY();

            return returnXY;
        }
    }
}
