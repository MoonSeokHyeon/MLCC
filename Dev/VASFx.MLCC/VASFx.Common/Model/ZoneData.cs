using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class ZoneData : IEntity, ICloneable
    { 
        [Required]
        public int Id { get; set; }

        public eExecuteZone Zone { get; set; }
        public int Score { get; set; }

        public virtual List<CalibrationData> CalibrationDatas { get; set; }
        public virtual ModelData ModelData { get; set; }

        public ZoneData()
        {
            this.Score = 60;
        }

        #region ICloneable
        protected virtual ZoneData ShallowCopy() => (ZoneData)this.MemberwiseClone();

        protected virtual ZoneData DeepCopy()
        {
            ZoneData clone = new ZoneData();
            clone.Zone = this.Zone;
            clone.Score = this.Score;
            clone.CalibrationDatas = this.CalibrationDatas;
            clone.ModelData = this.ModelData;

            return clone;
        }

        public ZoneData Clone()
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
