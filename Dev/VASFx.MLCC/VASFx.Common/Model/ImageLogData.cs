using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.Common.Model
{
    public class ImageLogData
    {
        public string FileName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Category { get; set; }
        public double Size { get; set; }
        public string Path { get; set; }
    }
}
