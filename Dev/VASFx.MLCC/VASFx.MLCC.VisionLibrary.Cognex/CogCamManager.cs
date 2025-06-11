using GSG.NET.Concurrent;
using GSG.NET.Excel;
using GSG.NET.Logging;
using GSG.NET.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Enum;
using VASFx.MLCC.Common.Interface;
using VASFx.MLCC.Common.VisionModel;
using VASFx.MLCC.Device.LightController;
using VASFx.MLCC.Device.LightController.Controller;

namespace VASFx.MLCC.VisionLibrary.Cognex
{
    public class CogCamManager : IDisposable
    {
        Logger logger = Logger.GetLogger();

        public Dictionary<eCamID, ICognexLib> CamsDic { get; private set; } = new Dictionary<eCamID, ICognexLib>();
        public List<LightConfig> LightList { get; private set; }
        public Dictionary<string, ILightController> LightControllerDic { get; private set; } = new Dictionary<string, ILightController>();

        public ISplashScreen SplashScreen { get; set; }

        ThreadCancel threadCancel = new ThreadCancel();

        public CogCamManager()
        {
        }

        void Init()
        {
            //this.threadCancel.AddGo(DoWork);
            GSG.NET.Quartz.QuartzUtils.Invoke("CAMCONNECTCHECKER", GSG.NET.Quartz.QuartzUtils.GetExpnSecond(1), CheckCamState);
        }

        //Thread 에서 Quartz 로 변경.
        void CheckCamState()
        {
            try
            {
                var camll = this.CamsDic.Values.ToList();
                camll.ForEach(c =>
                {
                    if (!c.IsConnected)
                    {
                        logger.I($"{c.Config.ID} - Disconnected State, Try Connecte");
                        //CogFrameGrabbers.Refresh();
                        if (c.ConnectCamera())
                            logger.I($"{c.Config.ID} - Connecte Success!");
                        else
                            logger.I($"{c.Config.ID} - Connecte Fail!");
                    }
                });
            }
            catch (Exception ex)
            {
                logger.E(ex);
            }
        }

        void DoWork()
        {
            while (!this.threadCancel.Canceled)
            {
                try
                {
                    LockUtils.Wait(1000);

                    var camll = this.CamsDic.Values.ToList();
                    camll.ForEach(c =>
                    {
                        LockUtils.Wait(1000);
                        if (!c.IsConnected)
                        {
                            logger.I($"{c.Config.ID} - Disconnected State, Try Connecte");
                            if (c.ConnectCamera())
                                logger.I($"{c.Config.ID} - Connecte Success!");
                            else
                                logger.I($"{c.Config.ID} - Connecte Fail!");
                        }
                    });
                }
                catch (Exception ex)
                {
                    logger.E(ex);
                }
            }
        }

        public void CreateCamera(List<CameraConfig> cams)
        {
            SplashScreen.SetRange(cams.Count);
            cams.ForEach(config =>
            {
                var cam = new CogLib(config);
                CamsDic.Add(config.ID, cam);
                cam.Init();
                if (cam.ConnectCamera())
                {
                    SplashScreen.AddMessage($"Connection Camera - {config.ID}");
                    logger.I($"Connection Camera - {config.ID}");
                }
                else
                {
                    SplashScreen.AddMessage($"Connection Fail Camera - {config.ID}");
                    logger.I($"Connection Fail Camera - {config.ID}");
                }

                SplashScreen.StepIt();
            });

            this.Init();
        }


        public void CreateCamera(string path)
        {
            var camList = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + path).Fetch<CameraConfig>("Cam").ToList();
            SplashScreen.SetRange(camList.Count);
            camList.ForEach(config =>
            {
                var cam = new CogLib(config);
                CamsDic.Add(config.ID, cam);
                cam.Init();
                if (cam.ConnectCamera())
                {
                    SplashScreen.AddMessage($"Connection Camera - {config.ID}");
                    logger.I($"Connection Camera - {config.ID}");
                }
                else
                {
                    SplashScreen.AddMessage($"Connection Fail Camera - {config.ID}");
                    logger.I($"Connection Fail Camera - {config.ID}");
                }
                SplashScreen.StepIt();
            });

            this.Init();
        }

        public void CreateLightController(string path)
        {
            var controllers = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + path).Fetch<VASFx.MLCC.Device.LightController.Config>("LightController").ToList();
            controllers.ForEach(c =>
            {
                var lc = new ALT(c);
                LightControllerDic.Add(lc.Config.Id, lc);
            });
        }

        public void CreateLight(string path)
        {
            LightList = new ExcelMapper(Path.Combine(System.Environment.CurrentDirectory) + path).Fetch<LightConfig>("Light").ToList();
        }

        public bool SetLightValue(eCamID camId, eLightPointKind lightPoint, int value)
        {
            var light = this.LightList.FirstOrDefault(l => l.CamID == camId && l.LightPointKind == lightPoint);
            var controller = this.LightControllerDic[light.ControllerID];

            Assert.NotNull(controller, "controller is null");

            return controller.LightOn(light.Chanel, value);
        }

        public bool SetLightOff(eCamID camId, eLightPointKind lightPoint)
        {
            var light = this.LightList.FirstOrDefault(l => l.CamID == camId && l.LightPointKind == lightPoint);
            var controller = this.LightControllerDic[light.ControllerID];

            Assert.NotNull(controller, "controller is null");

            return controller.LightOff(light.Chanel);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    //this.threadCancel.Cancel();
                    //LockUtils.Wait(500);
                    //this.threadCancel.StopWaitAll();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~CamManager() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
