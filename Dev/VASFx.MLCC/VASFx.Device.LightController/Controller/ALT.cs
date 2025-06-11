using GSG.NET.Logging;
using GSG.NET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.Device.LightController.Controller
{
    public class ALT : LightControllerBase
    {
        static Logger logger = Logger.GetLogger();

        int[] chennelValue = null;
        public override int[] ChennelValue { get => this.chennelValue; set => this.chennelValue = value; }

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

                //HACK: [3/25] group light off Debug Test
                //Close로 하면 1,2, ON 하면 3,4, OFF 되는 현상 발생함.
                //대처방안 생각중
                //Manual Doc Null => Pass ? 

                Close();
            }
        }

        override protected void WriteData(List<int> channel, List<int> val)
        {
            lock (this.lockObj)
            {
                int cnt = 0;
                channel.ForEach(x =>
                {
                    this.chennelValue[x - 1] = val[cnt];
                    cnt++;
                });

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
