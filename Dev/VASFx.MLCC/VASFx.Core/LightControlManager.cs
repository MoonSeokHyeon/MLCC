using GSG.NET.Excel;
using GSG.NET.Extensions;
using GSG.NET.Logging;
using GSG.NET.Utils;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Events;
using VASFx.Common.Model;
using VASFx.Common.Shared;
using VASFx.Device.LightController;
using VASFx.Device.LightController.Controller;
using VASFx.MLCC.Sqlite;

namespace VASFx.Core
{
    public class LightControlManager : IDisposable
    {
        Logger logger = Logger.GetLogger(typeof(LightControlManager));

        IContainerProvider provider;
        SqlManager sql = null;

        public IDictionary<int, ILightController> LightControllers = new Dictionary<int, ILightController>();
        //public IList<LightValueConfig> Lights = new List<LightValueConfig>();

        public LightControlManager(IContainerProvider containerProvider, IEventAggregator eventAggregator, SqlManager sql)
        {
            this.provider = containerProvider;
            this.sql = sql;

            eventAggregator.GetEvent<ApplicationExitEvent>().Subscribe((o) => Dispose(), true);
        }

        public void Dispose()
        {
        }

        public void CreateLightController()
        {
            var modelData = sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;
            var LightControllers = modelData.LightControllerDatas.ToList();

            Assert.NotNull(LightControllers, "LightController Manager Init - DB LightController Info is Null");

            var ll = LightControllers.OrderBy(x => x.PortNumber).ToList();

            ll.EachExt(c =>
            {
                var lightConfig = new Config();

                lightConfig.PortNo = c.PortNumber;
                lightConfig.BaudRate = c.BaudRate;
                lightConfig.pParity = (Parity)c.Parity;
                lightConfig.DataBits = c.DataBits;
                lightConfig.StopBits = (StopBits)c.StopBits;
                lightConfig.MaxChannel = c.MaxChannel;
                lightConfig.MaxVolume = c.MaxVolume;

                var lc = new ALT(lightConfig);
                this.LightControllers.Add(lightConfig.PortNo, lc);
            });
        }

        /// <summary>
        /// Model Change 이 후 한번은 실행해야 함.
        /// Initialization Controller Data
        /// Model Light Value Write To Controller
        /// </summary>
        public void InitControllerValue()
        {
        
        }

        public bool SetLightValue(int portNumner, int chnnel, int value)
        {
            //var light = this.Lights.FirstOrDefault(l => l.ZoneID == zoneID && l.GrabPos == grabPos);
            var lightData = this.LightControllers[portNumner];
            Assert.NotNull(lightData, "controller is null");

            return lightData.LightOn(chnnel, value);
        }

        public bool SetLightOff(int portNumner, int chnnel)
        {
            //var light = this.Lights.FirstOrDefault(l => l.ZoneID == zoneID && l.GrabPos == grabPos);
            var lightData = this.LightControllers[portNumner];
            Assert.NotNull(lightData, "controller is null");

            return lightData.LightOff(chnnel);
        }

        public bool SetLightValueMulti(int portNumber, List<int> channel, List<int> value)
        {
            var lightData = this.LightControllers[portNumber];
            Assert.NotNull(lightData, "controller is null");

            return lightData.LightOnMulti(channel, value);
        }

        public bool SetLightOffAllMulti(int portNumber)
        {
            var lightData = this.LightControllers[portNumber];
            Assert.NotNull(lightData, "controller is null");

            List<int> channel = new List<int>();
            List<int> value = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                channel.Add(i + 1);
                value.Add(0);
            }

            return lightData.LightOnMulti(channel, value);
        }

        public void SetInspectionLightOn()
        {
            var modelData = this.sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;

            var controllerDatas = modelData.LightControllerDatas.ToList();

            List<List<LightValueData>> lightDatas = new List<List<LightValueData>>();

            foreach (var controllerData in controllerDatas)
            {
                lightDatas.Add(controllerData.LightValues.FindAll(x => x.ZoneID == eExecuteZone.MLCC_INSPECTION));
            }

            List<int> channelList = new List<int>();
            List<int> valueList = new List<int>();

            foreach (var lightData in lightDatas)
            {
                channelList = new List<int>();
                valueList = new List<int>();
                var portNo = controllerDatas.FirstOrDefault(y => y.Id.Equals(lightData[0].LightControllerDatas.Id)).PortNumber;
                lightData.ForEach(x => channelList.Add(x.Channel));
                lightData.ForEach(x => valueList.Add(x.LightValue));

                this.SetLightValueMulti(portNo, channelList, valueList);
            }
        }
        public void SetInspectionLightOff()
        {
            var modelData = this.sql.SystemInfo.GetAll().FirstOrDefault().CurrentModel;

            var controllerDatas = modelData.LightControllerDatas.ToList();

            List<List<LightValueData>> lightDatas = new List<List<LightValueData>>();

            foreach (var controllerData in controllerDatas)
            {
                lightDatas.Add(controllerData.LightValues.FindAll(x => x.ZoneID == eExecuteZone.MLCC_INSPECTION));
            }

            List<int> channelList = new List<int>();
            List<int> valueList = new List<int>();

            foreach (var lightData in lightDatas)
            {
                var portNo = controllerDatas.FirstOrDefault(y => y.Id.Equals(lightData[0].LightControllerDatas.Id)).PortNumber;
                this.SetLightOffAllMulti(portNo);
            }
        }
    }
}
