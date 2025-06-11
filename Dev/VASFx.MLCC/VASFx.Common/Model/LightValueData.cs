using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class LightValueData : IEntity, ICloneable
    {
        public int Id { get ; set; }
        public int Channel { get; set; }
        public eExecuteZone ZoneID { get; set; }
        public eGrabPosition GrabPos { get; set; }
        public int LightValue { get; set; }

        public virtual LightControllerData LightControllerDatas { get; set; }

        #region IClone
        protected virtual LightValueData ShallowCopy() => (LightValueData)this.MemberwiseClone();

        protected virtual LightValueData DeepCopy()
        {
            LightValueData clone = new LightValueData();
            clone.Channel = this.Channel;
            clone.ZoneID = this.ZoneID;
            clone.GrabPos = this.GrabPos;
            clone.LightValue = this.LightValue;

            return clone;
        }

        public LightValueData Clone()
        {
            return DeepCopy();
        }

        object ICloneable.Clone()
        {
            return DeepCopy();
        }

        #endregion

    }
}
