using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Interface;

namespace VASFx.Common.Model
{
    public class OverlapInfo : IEntity
    {
        public int Id { get; set; }
        public double MinSize { get; set; }
        public double OverlapDouble { get; set; }
        public double OverlapTriple { get; set; }
        public double OverlapQuad { get; set; }
        public double OverlapPenta { get; set; }
        public double OverlapHexa { get; set; }
    }
}
