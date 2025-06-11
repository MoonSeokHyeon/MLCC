using GSG.NET.IO;
using GSG.NET.Logging;
using GSG.NET.Utils;
using System.Collections.Generic;

namespace VASFx.Device.LightController
{
    public abstract class LightControllerBase : ILightController
    {
        static Logger logger = Logger.GetLogger();

        internal SerialComm h = new SerialComm();
        public Config Config { get; set; }
        public virtual int[] ChennelValue { get; set; }

        abstract protected void WriteData(int chanel, int val);
        abstract protected void WriteData(List<int> channel, List<int> val);
        abstract protected int ReadData(int chanel);

        protected virtual void Open()
        {
            Assert.AreNotEqual(0, this.Config.PortNo, "PortNo Not Set");

            h.PortName = "COM" + this.Config.PortNo;
            h.BaudRate = this.Config.BaudRate;
            h.DataBits = this.Config.DataBits;
            h.Parity = this.Config.pParity;
            h.StopBits = this.Config.StopBits;
            h.ReadTimeout = 5000;

            h.Open();
        }

        protected virtual void Close() => h.Close();

        public LightControllerBase(Config config)
        {
            this.Config = config;
        }

        public LightControllerBase(int portNo)
        {
            this.Config = new Config()
            {
                PortNo = portNo,
                BaudRate = 9600,
                DataBits = 8,
                pParity = System.IO.Ports.Parity.Even,
                StopBits = System.IO.Ports.StopBits.None,
                MaxChannel = 8,
                MaxVolume = 255,
            };
        }

        public bool LightOffAllChanel()
        {
            bool result = true;

            for (int i = 0; i < Config.MaxChannel; i++)
            {
                if (!this.LightOff(i))
                {
                    result = false;
                }
            }

            return result;
        }

        public bool LightOff(int chanel)
        {
            bool result = false;
            try
            {
                this.WriteData(chanel, 0);
                result = true;
            }
            catch (System.Exception ex)
            {
                this.Close();

                logger.E($"Light Off Error - Chanel {chanel}");
                logger.E(ex);
            }

            return result;
        }

        public bool LightOn(int chanel, int val)
        {
            bool result = false;
            try
            {
                this.WriteData(chanel, val);
                result = true;
            }
            catch (System.Exception ex)
            {
                this.Close();

                logger.E($"Light On Error - Chanel {chanel}");
                logger.E(ex);
            }

            return result;
        }

        public bool LightOnMulti(List<int> channel, List<int> val)
        {
            bool result = false;
            try
            {
                this.WriteData(channel, val);
                result = true;
            }
            catch (System.Exception ex)
            {
                this.Close();

                logger.E($"Multi Light On Error");
                logger.E(ex);
            }

            return result;
        }

        public int GetChanelValue(int chanel)
        {
            int ret = 0;

            return ret;
        }
    }
}
