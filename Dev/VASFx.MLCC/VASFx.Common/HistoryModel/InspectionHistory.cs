using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.Common.HistoryModel
{
    public class InspectionHistory : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 문자 내용을 기록한다.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ZoneID 
        /// History 발생 작업 구역을 나타냄.
        /// </summary>
        public eExecuteZone Zone { get; set; }

        public DateTime CreateDate { get; set; }
        public eMesh MeshId { get; set; }
        public double Area { get; set; }
        public bool IsExistence { get; set; }
        public bool IsOverlap { get; set; }
        public InspectionHistory()
        {
            CreateDate = DateTime.Now;
        }
    }
}
