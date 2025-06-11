using GSG.NET.Logging;
using GSG.NET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Device.LightController.Controller
{
    public class ALP : LightControllerBase
    {
        static Logger logger = Logger.GetLogger();

        #region Construct
        public ALP(int portNo = 1) : base(portNo)
        {
        }

        public ALP(Config config) : base(config) { }

        #endregion

        object lockObj = new object();
        override protected void WriteData(int chanel, int val)
        {
            lock (this.lockObj)
            {
                this.Open();

                var mb = new MemoryBuffer(64);
                mb.AppendAscii("[");
                mb.AppendAscii(chanel.ToString("00"));
                mb.AppendAscii(val.ToString("000"));
                mb.AppendAscii("/r/n");

                this.h.Write(mb.ToBytes);

                this.Close();
            }
        }

        override protected int ReadData(int chanel)
        {
            return 0;
        }
    }
}
