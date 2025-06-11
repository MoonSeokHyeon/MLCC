using Basler.Pylon;
using GSG.NET.Concurrent;
using GSG.NET.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using GSG.NET.Utils;

namespace VASFx.Device.Camera.Cameras
{
    public class BaslerCam : Common.Interface.ICamera
    {
        static Logger logger = Logger.GetLogger(typeof(BaslerCam));

        public CameraInfo CamInfo { get; private set; }

        #region Events
        public event Action<eCamID> ConnectionLost;
        public event Action<eCamID> Connected;
        public event Action<eCamID> CameraClosed;
        public event Action<byte[], eCamID> ImageGrabbed;
        public event Action<eCamID> GrabStarted;
        public event Action<eCamID> GrabStopped;
        #endregion

        #region Properties
        public bool IsConnected
        {
            get
            {
                if (camera == null)
                    return false;
                return camera.IsConnected;
            }
        }

        public eCamID CamID { get; }

        byte[] _latestFrame = null;
        public byte[] LatestFrame
        {
            get
            {
                lock (_lockLatestFrame)
                {
                    if (_latestFrame == null) return null;

                    byte[] rtByteArray = new byte[_latestFrame.Length];
                    System.Buffer.BlockCopy(_latestFrame, 0, rtByteArray, 0, _latestFrame.Length);
                    return rtByteArray;
                }
            }
            private set
            {
                lock (_lockLatestFrame)
                {
                    _latestFrame = value;
                }
            }
        }

        public bool IsGrabbing { get => this.camera.StreamGrabber.IsGrabbing; }

        public int Width { get; set; }
        public int Height { get; set; }
        #endregion

        Basler.Pylon.Camera camera = null;
        PixelDataConverter converter = null;

        object _lockLatestFrame = new object();

        ThreadCancel _threadCancel = new ThreadCancel();

        #region Construct
        public BaslerCam(CameraInfo info)
        {
            this.CamInfo = info;
            this.CamID = this.CamInfo.CamID;

            converter = new PixelDataConverter();
        }

        public bool DestoryCamera()
        {
            this._threadCancel.Cancel();
            this._threadCancel.StopWaitAll();

            if (camera == null)
                return false;

            if (camera.StreamGrabber.IsGrabbing)
                camera.StreamGrabber.Stop();

            try
            {
                camera.Close();
                camera.Dispose();
                camera = null;

                return true;
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return false;
            }
        }
        #endregion

        #region Private Method
        public List<ICameraInfo> UpdateList()
        {
            List<ICameraInfo> deviceList = new List<ICameraInfo>();

            try
            {
                deviceList = CameraFinder.Enumerate();
            }
            catch (Exception ex)
            {
                //logger.E($"Excpetion {BaslerCam.CamID}: Update List Method - {ex}");
                return null;
            }

            return deviceList;
        }

        public bool IsOpen()
        {
            if (camera == null)
                return false;

            return camera.IsOpen;
        }
        #endregion

        #region Public
        public bool Connect()
        {
            try
            {
                var list = UpdateList();
                ICameraInfo info = null;
                foreach (var tag in list)
                {
                    if (tag[CameraInfoKey.SerialNumber] == this.CamInfo.SerialNumber)
                    {
                        info = tag; break;
                    }
                }
                if (info == null) return false;

                camera = new Basler.Pylon.Camera(info);

                camera.CameraOpened += Configuration.AcquireSingleFrame;
                camera.ConnectionLost += OnConnectionLost;
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                camera.StreamGrabber.GrabStarted += OnGrabStarted;
                camera.StreamGrabber.GrabStopped += OnGrabStopped;
                camera.CameraClosed += Camera_CameraClosed;

                camera.Open();

                CamInfo.SerialNumber = info[CameraInfoKey.SerialNumber];

                camera.Parameters[PLCamera.GammaEnable].SetValue(true);
                camera.Parameters[PLCamera.GammaSelector].SetValue("User");
                camera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(1);
                //camera.Parameters[PLCamera.ReverseX].SetValue(true);
                //camera.Parameters[PLCamera.ReverseY].SetValue(true);

                ThreadUtils.Invoke(th_DoContinuousGrab);

                //Configuration.AcquireContinuous(camera, null);

                this.Width = (int)GetWidth();
                this.Height = (int)GetHeight();

                return true;
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {this.CamID}: - {ex}");
                return false;
            }
        }

        #region Grab Method
        public void Grab()
        {
            if (camera == null || !camera.IsConnected)
                return;

            if (camera.StreamGrabber.IsGrabbing)
                StopGrabContinuous();

            try
            {
                Configuration.AcquireSingleFrame(camera, null);
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                camera.StreamGrabber.Stop();
            }
        }

        public byte[] GrabBuffer { get; set; }
        public object GrabOneShot()
        {
            if (camera == null)
                return null;

            if (!camera.IsConnected)
                Connect();

            if (camera.StreamGrabber.IsGrabbing)
                StopGrabContinuous();

            Bitmap retImage = null;
            byte[] imageData = null;

            try
            {
                lock (_lockImageGrabbed)
                {
                    //camera.StreamGrabber.Start();

                    var grabResult = camera.StreamGrabber.GrabOne(5000);//camera.StreamGrabber.RetrieveResult(5000, TimeoutHandling.ThrowException);
                    using (grabResult)
                    {
                        if (grabResult == null) return null;

                        if (grabResult.GrabSucceeded)
                        {
                            var temp = grabResult.PixelData as byte[];
                            imageData = new byte[temp.Length];
                            Marshal.Copy(grabResult.PixelDataPointer, imageData, 0, temp.Length);

                            GrabBuffer = new byte[imageData.Length];
                            System.Buffer.BlockCopy(imageData, 0, GrabBuffer, 0, imageData.Length);

                            //System.Buffer.BlockCopy(temp, 0, imageData, 0, temp.Length);

                            //using (var ms = new MemoryStream(imageData))
                            //{
                            //    var rotateImage = Image.FromStream(ms);
                            //    rotateImage.RotateFlip(this.CamInfo.ImageRotateType);
                            //    rotateImage.Save(ms, rotateImage.RawFormat);
                            //    imageData = ms.ToArray();
                            //}

                            //retImage = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                            //BitmapData bmpData = retImage.LockBits(new Rectangle(0, 0, retImage.Width, retImage.Height), ImageLockMode.ReadWrite, retImage.PixelFormat);
                            //converter.OutputPixelFormat = PixelType.BGRA8packed;
                            //IntPtr ptrBmp = bmpData.Scan0;
                            //converter.Convert(ptrBmp, bmpData.Stride * retImage.Height, grabResult);
                            //retImage.UnlockBits(bmpData);
                            //retImage.RotateFlip(this.CamInfo.ImageRotateType);
                            //this.LatestFrame = retImage;
                        }
                        else
                            logger.E($"Grab Errer: {grabResult.ErrorCode} {grabResult.ErrorDescription}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return null;
            }
            finally
            {
                //if (camera != null)
                //camera.StreamGrabber.Stop();
            }

            return imageData;
        }

        public byte[] ToByteArray(byte[] o)
        {
            int size = Marshal.SizeOf(o.Length);
            byte[] buffer = new byte[size];
            IntPtr p = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(o, p, false);
                Marshal.Copy(p, buffer, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(p);
            }
            return buffer;
        }

        public bool GrabContinuous()
        {
            if (camera == null || !camera.IsConnected)
                return false;

            if (camera.StreamGrabber.IsGrabbing)
                StopGrabContinuous();

            try
            {
                Configuration.AcquireContinuous(camera, null);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);

                return true;
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return false;
            }
        }

        public bool StopGrabContinuous()
        {
            if (camera == null || !camera.IsConnected)
                return false;

            try
            {
                camera.StreamGrabber.Stop();
                GSG.NET.Concurrent.LockUtils.Wait(100);
                while (camera.StreamGrabber.IsGrabbing)
                    GSG.NET.Concurrent.LockUtils.Wait(5);

                return true;
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return false;
            }
        }
        #endregion

        #region Parameter Set
        public bool SetDigitalShift(int value)
        {
            if (camera == null || !camera.IsConnected)
                return false;

            try
            {
                camera.Parameters[PLCamera.DigitalShift].SetValue(value);
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return false;
            }

            return true;
        }

        public bool SetGainRaw(int value)
        {
            if (camera == null || !camera.IsConnected)
                return false;

            try
            {
                camera.Parameters[PLCamera.GainRaw].SetValue(value);
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return false;
            }

            return true;
        }

        public bool SetGainRawAuto(bool value)
        {
            if (camera == null || !camera.IsConnected)
                return false;

            string Value;
            if (value)
                Value = "Continuous";
            else
                Value = "Off";

            try
            {
                camera.Parameters[PLCamera.GainAuto].SetValue(Value);
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID} - {ex}");
                return false;
            }

            return true;
        }

        public bool SetExposureTimeAbs(double value)
        {
            if (camera == null || !camera.IsConnected)
                return false;

            try
            {
                camera.Parameters[PLCamera.ExposureTimeAbs].SetValue(value);
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID} - {ex}");
                return false;
            }

            return true;
        }

        public bool SetGamma(double value)
        {
            if (camera == null || !camera.IsConnected)
                return false;

            try
            {
                camera.Parameters[PLCamera.Gamma].SetValue(value);
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID} - {ex}");
                return false;
            }

            return true;
        }
        #endregion

        #region Parameter Get
        public int GetDigitalShift()
        {
            if (camera == null || !camera.IsConnected)
                return -1;

            long value;

            try
            {
                value = camera.Parameters[PLCamera.DigitalShift].GetValue();
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return -1;
            }

            return (int)value;
        }

        public int GetGainRaw()
        {
            if (camera == null || !camera.IsConnected)
                return -1;

            long value;

            try
            {
                value = camera.Parameters[PLCamera.GainRaw].GetValue();
            }
            catch (Exception ex)
            {

                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return -1;
            }

            return (int)value;
        }

        public double GetExposureTimeAbs()
        {
            if (camera == null || !camera.IsConnected)
                return -1;

            double value;

            try
            {
                value = camera.Parameters[PLCamera.ExposureTimeAbs].GetValue();
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID} - {ex}");
                return -1;
            }

            return value;
        }

        public double GetGamma()
        {
            if (camera == null || !camera.IsConnected)
                return -1;

            double value;

            try
            {
                camera.Parameters[PLCamera.GammaEnable].SetValue(true);
                camera.Parameters[PLCamera.GammaSelector].SetValue("User");

                value = camera.Parameters[PLCamera.Gamma].GetValue();
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID} - {ex}");
                return -1;
            }

            return value;
        }

        public string GetCameraIP()
        {
            if (camera == null || !camera.IsConnected)
                return "";

            string value;

            try
            {
                value = camera.CameraInfo[CameraInfoKey.DeviceIpAddress];
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return String.Empty;
            }

            return value;
        }

        public string GetCameraModel()
        {
            if (camera == null || !camera.IsConnected)
                return "";

            string value;

            try
            {
                value = camera.Parameters[PLCamera.DeviceModelName].GetValue();
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return "";
            }

            return value;
        }

        public string GetSerialNumber()
        {
            if (camera == null || !camera.IsConnected)
                return "";

            string value;

            try
            {
                value = camera.Parameters[PLCamera.DeviceSerialNumber].GetValue();
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return String.Empty;
            }

            return value;
        }

        public long GetWidth()
        {
            if (camera == null || !camera.IsConnected)
                return -1;

            long value;

            try
            {
                value = camera.Parameters[PLCamera.Width].GetValue();
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return -1;
            }

            return value;
        }

        public long GetHeight()
        {
            if (camera == null || !camera.IsConnected)
                return -1;

            long value;

            try
            {
                value = camera.Parameters[PLCamera.Height].GetValue();
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
                return -1;
            }

            return value;
        }
        #endregion

        #endregion

        #region Camera Event Chain
        private void Camera_CameraClosed(object sender, EventArgs e)
        {
            this.CameraClosed?.Invoke(this.CamID);
        }

        private void Camera_CameraOpened(object sender, EventArgs e)
        {
            this.Connected?.Invoke(CamID);
        }

        private void OnGrabStarted(object sender, System.EventArgs e)
        {
            GrabStarted?.Invoke(this.CamID);
        }

        private void OnGrabStopped(object sender, GrabStopEventArgs e)
        {
            GrabStopped?.Invoke(this.CamID);
        }
        private void OnConnectionLost(object sender, System.EventArgs e)
        {
            DestoryCamera();
            ConnectionLost?.Invoke(this.CamID);
        }

        object _lockImageGrabbed = new object();
        private void OnImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                lock (this._lockImageGrabbed)
                {
                    IGrabResult grabResult = e.GrabResult;

                    if (grabResult == null) return;

                    if (grabResult.GrabSucceeded)
                    {
                        //Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        //BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        //converter.OutputPixelFormat = PixelType.BGRA8packed;
                        //IntPtr ptrBmp = bmpData.Scan0;
                        //converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult);
                        //bitmap.UnlockBits(bmpData);
                        //bitmap.RotateFlip(this.CamInfo.ImageRotateType);

                        var temp = grabResult.PixelData as byte[];
                        var targetImage = new byte[temp.Length];
                        Marshal.Copy(grabResult.PixelDataPointer, targetImage, 0, temp.Length);
                        LatestFrame = targetImage;

                        GSG.NET.Utils.DelegateUtils.Invoke(ImageGrabbed, LatestFrame, CamInfo.CamID);
                    }
                    else
                        logger.E($"Grab Errer: {grabResult.ErrorCode} {grabResult.ErrorDescription}");
                }
            }
            catch (Exception ex)
            {
                logger.E($"Excpetion {CamInfo.CamID}: - {ex}");
            }
            finally
            {
                e.DisposeGrabResultIfClone();
            }
        }

        #endregion

        #region Thread Method
        bool _isContinuousGrab = false;
        public bool SetContinuousGrab(bool isSet)
        {
            this._isContinuousGrab = isSet;
            return true;
        }

        void th_DoContinuousGrab()
        {
            while (!this._threadCancel.Canceled)
            {
                GSG.NET.Concurrent.LockUtils.Wait(5);
                try
                {
                    if (!_isContinuousGrab) continue;
                    if (!this.camera.IsConnected) continue;

                    var st = SwUtils.CurrentTimeMillis;
                    var rtBitmap = this.GrabOneShot();

                    GSG.NET.Utils.DelegateUtils.Invoke(ImageGrabbed, rtBitmap, CamInfo.CamID);
                    logger.I($"Cam {this.CamID} - End Gab {SwUtils.Elapsed(st)}");
                }
                catch (Exception ex)
                {
                }

            }

        }
        #endregion

    }
}
