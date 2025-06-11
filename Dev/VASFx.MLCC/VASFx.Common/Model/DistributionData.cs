using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.Common.Model
{
    public class DistributionData 
    {
        public string DateTime { get; set; }

        public string Report { get; set; }

        public string Total { get; set; }

        public string OK { get; set; }

        public string NG { get; set; }

        public DistributionData()
        {
            DateTime = "";
            Report = "";
            Total = "";
            OK = "";
            NG = "";
        }
    }
}
