using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.Device.LightController
{
    public interface ILightController
    {
        int[] ChennelValue { get; set; }
        bool LightOn(int chanel, int val);
        bool LightOff(int chanel);
        bool LightOnMulti(List<int> channel, List<int> val);
        int GetChanelValue(int chanel);
    }
}
