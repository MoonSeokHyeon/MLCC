using GSG.NET.Logging;
using GSG.NET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Device.LightController.Controller
{
    public class ALT : LightControllerBase
    {
        static Logger logger = Logger.GetLogger();

        int[] chennelValue = null;

        #region Construct

        public ALT(Config config) : base(config)
        {
            this.chennelValue = new int[config.MaxChannel];
            for (int i = 0; i < this.chennelValue.Length; i++)
            {
                this.chennelValue[i] = 0;
            }
        }

        public ALT(int portNo) : base(portNo)
        {
        }

        #endregion

        object lockObj = new object();
        override protected void WriteData(int chanel, int val)
        {
            lock (this.lockObj)
            {
                Assert.IsTrue(chanel > 0, "Channel No must not be less than zero.");
                this.chennelValue[chanel - 1] = val;

                Open();

                var mb = new MemoryBuffer();
                mb.Append(0xEF);
                mb.Append(0xEF);
                mb.Append(0);
                for (int i = 0; i < this.chennelValue.Length; i++)
                {
                    mb.Append(this.chennelValue[i]);
                }

                int checksum = 0x00;
                for (int i = 0; i < this.chennelValue.Length; i++)
                {
                    if (i == this.chennelValue.Length - 1)
                        checksum ^= this.chennelValue[i] + 0x01;
                    else
                        checksum ^= this.chennelValue[i];
                }
                mb.Append(checksum);
                mb.Append(0xEE);
                mb.Append(0xEE);

                this.h.Write(mb.ToBytes);
                GSG.NET.Concurrent.LockUtils.Wait(100);

                if (this.h.IsReadDataExist)
                {
                    h.ReadBytes(3);//Head 내용 삭제.
                    for (int i = 0; i < this.chennelValue.Length; i++)
                    {
                        this.chennelValue[i] = h.Read1Byte();
                    }
                }
                else
                    Assert.Fail($"ComPot {this.Config.PortNo} - Receive Data is Empty");

                Close();
            }
        }

        override protected int ReadData(int chanel)
        {
            return this.chennelValue[chanel];
        }

        byte GetCheckSum(byte[] bs)
        {
            byte rb = 0;
            foreach (var item in bs)
            {
            }
            return (byte)(rb & 0xf); //&0xff 수정.
        }
    }
}
