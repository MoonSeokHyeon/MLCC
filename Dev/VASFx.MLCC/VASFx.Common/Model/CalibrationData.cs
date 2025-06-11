using GSG.NET.Vision.Model;
using System;
using System.ComponentModel.DataAnnotations;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class CalibrationData : IEntity, ICloneable
    { 
        [Required]
        public int Id { get; set; }
        public eExecuteZone Zone { get; set; }
        public eGrabPosition GrabPos { get; set; }
        public XY XAxisStartXY { get; set; }
        public XY XAxisEndXY { get; set; }
        public XY YAxisStartXY { get; set; }
        public XY YAxisEndXY { get; set; }
        public XY FixPosXY { get; set; }
        public XY ResolutionXY { get; set; }
        public XY BasePosXY { get; set; }

        public virtual ZoneData ZoneData { get; set; }

        public CalibrationData()
        {
            this.XAxisStartXY = new XY();
            this.XAxisEndXY = new XY();
            this.YAxisStartXY = new XY();
            this.YAxisEndXY = new XY();
            this.FixPosXY = new XY();
            this.ResolutionXY = new XY();
            this.BasePosXY = new XY();
        }

        #region ICloneable
        protected virtual CalibrationData ShallowCopy() => (CalibrationData)this.MemberwiseClone();

        protected virtual CalibrationData DeepCopy()
        {
            CalibrationData clone = new CalibrationData();
            clone.Zone = this.Zone;
            clone.Id = this.Id;
            clone.GrabPos = this.GrabPos;
            clone.XAxisStartXY = this.XAxisStartXY;
            clone.XAxisEndXY= this.XAxisEndXY;
            clone.YAxisStartXY= this.YAxisStartXY;
            clone.YAxisEndXY= this.YAxisEndXY;
            clone.FixPosXY= this.FixPosXY;
            clone.ResolutionXY= this.ResolutionXY;
            clone.BasePosXY= this.BasePosXY;
            clone.ZoneData = this.ZoneData.Clone();

            return clone;
        }

        public CalibrationData Clone()
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
