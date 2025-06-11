using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Device.LightController
{
    public interface ILightController
    {
        bool LightOn(int chanel, int val);
        bool LightOff(int chanel);
        int GetChanelValue(int chanel);
    }
}
