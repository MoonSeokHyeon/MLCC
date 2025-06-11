using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VASFx.Common.Interface;

namespace VASFx.Common.Model
{
    public class ModelData : IEntity, ICloneable
    {
        public int Id { get; set; }

        [Index]
        [Required]
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }

        public ModelData() => this.CreateTime = DateTime.Now;

        public virtual List<ZoneData> ZoneDatas { get; set; }
        public virtual List<LightControllerData> LightControllerDatas { get; set; }

        #region ICloneable
        protected virtual ModelData ShallowCopy() => (ModelData)this.MemberwiseClone();

        protected virtual ModelData DeepCopy()
        {
            ModelData clone = new ModelData();
            clone.Name = this.Name;
            clone.Description = this.Description;
            clone.ZoneDatas = this.ZoneDatas.ConvertAll(o => o.Clone());
            clone.LightControllerDatas = this.LightControllerDatas.ConvertAll(o => o.Clone());

            return clone;
        }

        public ModelData Clone()
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
