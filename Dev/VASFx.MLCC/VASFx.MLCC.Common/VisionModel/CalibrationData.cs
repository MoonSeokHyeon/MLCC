using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Common.VisionModel
{
    public class CalibrationData : ICloneable
    {
        public int Id { get; set; }

        public XY XAxisStartXY { get; set; }
        public XY XAxisEndXY { get; set; }
        public XY YAxisStartXY { get; set; }
        public XY YAxisEndXY { get; set; }
        public XY FixPosXY { get; set; }
        public XY ResolutionXY { get; set; }
        public XY BasePosXY { get; set; }

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
            clone.XAxisStartXY = new XY(this.XAxisStartXY);
            clone.XAxisEndXY = new XY(this.XAxisEndXY);
            clone.YAxisStartXY = new XY(this.YAxisStartXY);
            clone.YAxisEndXY = new XY(this.YAxisEndXY);

            clone.FixPosXY = new XY(this.FixPosXY);
            clone.BasePosXY = new XY(this.BasePosXY);
            clone.ResolutionXY = new XY(this.ResolutionXY);

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
