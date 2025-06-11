using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class JobData : IEntity
    {
        public int Id { get; set; }

        public eExecuteZone ZoneID { get; set; }

        public eGrabPosition GrabPos { get; set; }
    }
}
