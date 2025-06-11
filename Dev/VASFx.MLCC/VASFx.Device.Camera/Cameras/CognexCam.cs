using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using GSG.NET.Concurrent;
using GSG.NET.Logging;
using GSG.NET.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Interface;
using VASFx.Common.Model;
using VASFx.Common.Shared;

namespace VASFx.Device.Camera.Cameras
{
    public class CognexCam : ICamera
    {
        Logger logger = Logger.GetLogger();

        private ICogAcqFifo fifo = null;
        public ICogAcqFifo Fifo
        {
            get { return fifo; }
        }

        public bool IsConnected { get; set; }
        public int Width { get => this._cameraInfo.Width; }
        public int Height { get => this._cameraInfo.Height; }

        public byte[] GrabBuffer => throw new System.NotImplementedException();

        public CameraInfo CamInfo => this._cameraInfo;

        public eCamID CamID => this._cameraInfo.CamID;

        public bool IsGrabbing { get; private set; } = false;

        CameraInfo _cameraInfo = null;

        public event Action<eCamID> ConnectionLost;
        public event Action<eCamID> Connected;
        public event Action<eCamID> CameraClosed;
        public event Action<byte[], eCamID> ImageGrabbed;
        public event Action<eCamID> GrabStarted;
        public event Action<eCamID> GrabStopped;
        public event Action<ICogImage> CogImageGrabbed;

        TaskCancel _taskCancel = new TaskCancel();

        public CognexCam(CameraInfo info)
        {
            _cameraInfo = info;
        }

        public bool ConnectCamera(string SerialNo)
        {
            var frameGrabbers = new CogFrameGrabbers();

            try
            {
                var grabber = frameGrabbers.Cast<ICogFrameGrabber>().FirstOrDefault(x => x.SerialNumber.Equals(SerialNo));
                if (grabber == null) return false;

                this.fifo = grabber.CreateAcqFifo("Generic GigEVision (Mono)", CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
                if (this.fifo == null)
                    this.IsConnected = false;
            }
            catch (System.Exception ex)
            {
                this.fifo = null;
                return false;
            }
            this.IsConnected = true;

            return true;
        }

        public void DisconnectCamera()
        {
            if (this.fifo != null) this.Fifo.FrameGrabber.Disconnect(false);
        }

        public ICogImage GrabImageCamera()
        {
            //var state = this.Fifo.FrameGrabber.GetStatus(false);

            int trigger = 0;
            ICogImage grabImage;

            if (fifo != null)
            {
                try
                {
                    grabImage = fifo.Acquire(out trigger);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            var rotate = CogIPOneImageFlipRotateOperationConstants.None;

            switch (this._cameraInfo.ImageRotateType)
            {
                case RotateFlipType.RotateNoneFlipNone:
                    rotate = CogIPOneImageFlipRotateOperationConstants.None;
                    break;
                case RotateFlipType.Rotate90FlipNone:
                    rotate = CogIPOneImageFlipRotateOperationConstants.Rotate90Deg;
                    break;
                case RotateFlipType.Rotate180FlipNone:
                    rotate = CogIPOneImageFlipRotateOperationConstants.Rotate180Deg;
                    break;
                case RotateFlipType.Rotate270FlipNone:
                    rotate = CogIPOneImageFlipRotateOperationConstants.Rotate270Deg;
                    break;
                case RotateFlipType.RotateNoneFlipX:
                    rotate = CogIPOneImageFlipRotateOperationConstants.Flip;
                    break;
                case RotateFlipType.Rotate90FlipX:
                    rotate = CogIPOneImageFlipRotateOperationConstants.FlipAndRotate90Deg;
                    break;
                case RotateFlipType.Rotate180FlipX:
                    rotate = CogIPOneImageFlipRotateOperationConstants.FlipAndRotate180Deg;
                    break;
                case RotateFlipType.Rotate270FlipX:
                    rotate = CogIPOneImageFlipRotateOperationConstants.FlipAndRotate270Deg;
                    break;
                default:
                    break;
            }

            var IpOneImageTool = new CogIPOneImageTool();
            IpOneImageTool.Operators.Add(new CogIPOneImageFlipRotate() { OperationInPixelSpace = rotate });
            IpOneImageTool.InputImage = grabImage;
            IpOneImageTool.Run();

            IpOneImageTool.OutputImage.PixelFromRootTransform = new CogTransform2DLinear(); //! 회전 후 이미지 좌상단을 0, 0으로 다시 만든다.
            ICogImage ret = IpOneImageTool.OutputImage;
            IpOneImageTool.Dispose();

            return ret;
        }

        public bool DestoryCamera()
        {
            if (IsGrabbing)
                this.StopGrabContinuous();

            this.DisconnectCamera();
            return true;
        }

        public void Grab()
        {
        }

        public object GrabOneShot()
        {
            return this.GrabImageCamera();
        }

        public bool GrabContinuous()
        {
            var task = Task4.Go(() =>
            {
                this.IsGrabbing = true;
                this.GrabStarted?.Invoke(this.CamID);
                while (!_taskCancel.Canceled)
                {
                    LockUtils.Wait(5);
                    try
                    {
                        var image = this.GrabImageCamera();
                        DelegateUtils.Invoke(CogImageGrabbed, image);
                    }
                    catch (Exception ex)
                    {
                        logger.E($"Camera {this.CamID} - Continuous Grab Exception [{ex}]");
                    }
                }
                this.IsGrabbing = false;
                this.GrabStopped?.Invoke(this.CamID);
            });
            _taskCancel.Add(task);

            return true;
        }

        public bool StopGrabContinuous()
        {
            _taskCancel.Cancel();
            _taskCancel.WaitAll();

            return true;
        }

        public bool Connect()
        {
            return this.ConnectCamera(this._cameraInfo.SerialNumber);
        }
    }
}
