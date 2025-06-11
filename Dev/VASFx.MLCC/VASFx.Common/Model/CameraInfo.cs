using System.Drawing;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class CameraInfo : IEntity
    {
        public int Id { get; set; }

        public eCamID CamID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string DeviceIpAddress { get; set; }
        public string SerialNumber { get; set; }
        public string DeviceType { get; set; }
        public string ModelName { get; set; }

        public int DigitalShift { get; set; }
        public int GainRaw { get; set; }
        public double ExposureTimeAbs { get; set; }
        public double Gamma { get; set; }

        public RotateFlipType ImageRotateType { get; set; }

        public CameraInfo()
        {
            Width = 3840;
            Height = 2748;
            DeviceIpAddress = "192.168.0.0";
            SerialNumber = "99999999";
            DeviceType = "GigE";
            ModelName = "acA3800-10gm";

            DigitalShift = 0;
            GainRaw = 51;
            ExposureTimeAbs = 35000;
            Gamma = 1.0;
        }
    }
}
