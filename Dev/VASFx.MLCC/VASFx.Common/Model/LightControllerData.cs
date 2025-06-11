using System;
using System.Collections.Generic;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class LightControllerData : IEntity, ICloneable
    {
        public int Id { get; set; }
        public int PortNumber { get; set; }
        public int BaudRate { get; set; }
        public int Parity { get; set; }
        public int DataBits { get; set; }
        public int StopBits { get; set; }
        public int MaxChannel { get; set; }
        public int MaxVolume { get; set; }

        public virtual List<LightValueData> LightValues { get; set; } = new List<LightValueData>();

        public virtual ModelData ModelData { get; set; }

        #region ICloneable
        protected virtual LightControllerData ShallowCopy() => (LightControllerData)this.MemberwiseClone();

        protected virtual LightControllerData DeepCopy()
        {
            LightControllerData clone = new LightControllerData();
            clone.PortNumber = this.PortNumber;
            clone.BaudRate = this.BaudRate;
            clone.Parity = this.Parity;
            clone.DataBits = this.DataBits;
            clone.StopBits = this.StopBits;
            clone.MaxChannel = this.MaxChannel;
            clone.MaxVolume = this.MaxVolume;
            clone.LightValues = this.LightValues;
            clone.ModelData = this.ModelData;

            return clone;
        }

        public LightControllerData Clone()
        {
            return ShallowCopy();
        }

        object ICloneable.Clone()
        {
            return DeepCopy();
        }

        #endregion
 
    }
}
