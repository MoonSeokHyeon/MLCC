using GSG.NET.Logging;
using GSG.NET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Device.LightController.Controller
{
    public class LVS : LightControllerBase
    {
        static Logger logger = Logger.GetLogger();
        object lockObj = new object();

        #region Construct
        public LVS(int portNo) : base(portNo) { }

        public LVS(Config config) : base(config) { }

        #endregion

        protected override int ReadData(int chanel)
        {
            throw new NotImplementedException();
        }

        protected override void WriteData(int chanel, int val)
        {
            lock (this.lockObj)
            {
                this.Open();

                var mb = new MemoryBuffer(64);
                mb.AppendAscii("L");
                mb.AppendAscii(chanel.ToString("00"));
                mb.AppendAscii(val.ToString("000"));
                mb.AppendAscii("/r/n");

                this.h.Write(mb.ToBytes);

                this.Close();
            }
        }
    }
}
