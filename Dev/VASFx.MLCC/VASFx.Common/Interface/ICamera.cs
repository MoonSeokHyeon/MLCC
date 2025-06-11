using System;
using System.Drawing;
using VASFx.Common.Model;
using VASFx.Common.Shared;

namespace VASFx.Common.Interface
{
    public interface ICamera
    {
        event Action<eCamID> ConnectionLost;
        event Action<eCamID> Connected;
        event Action<eCamID> CameraClosed;
        event Action<byte[], eCamID> ImageGrabbed;
        event Action<eCamID> GrabStarted;
        event Action<eCamID> GrabStopped;

        int Width { get; }
        int Height { get; }
        byte[] GrabBuffer { get; }

        CameraInfo CamInfo { get; }
        eCamID CamID { get; }
        bool IsConnected { get; }
        bool IsGrabbing { get; }
        bool DestoryCamera();
        void Grab();
        object GrabOneShot();
        bool GrabContinuous();
        bool StopGrabContinuous();
        bool Connect();
    }
}
