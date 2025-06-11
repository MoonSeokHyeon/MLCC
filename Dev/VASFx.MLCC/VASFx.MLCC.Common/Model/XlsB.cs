using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Enum;

namespace VASFx.MLCC.Common.Model
{
    public class XlsB
    {
        public int Addr { get; set; }
        public string TagName { get; set; }
        public string SubText { get; set; }
        public int SubNo { get; set; }
        public int CallbackOrder { get; set; }
        public ePLCKind Kind { get; set; }
        public string Comment { get; set; }
        public bool AutoOff { get; set; }
    }
}
