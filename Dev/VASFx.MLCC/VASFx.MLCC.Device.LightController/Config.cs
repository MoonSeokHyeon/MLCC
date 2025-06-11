using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Device.LightController
{
    public class Config
    {
        public string Id { get; set; }
        public int PortNo { get; set; }
        public int BaudRate { get; set; }
        public Parity pParity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public int MaxChannel { get; set; }
        public int MaxVolume { get; set; }
    }
}
