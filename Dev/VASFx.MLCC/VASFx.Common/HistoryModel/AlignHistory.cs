using GSG.NET.Vision.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.Common.Interface;
using VASFx.Common.Shared;

namespace VASFx.Common.HistoryModel
{
    public class AlignHistory : IEntity
    {
        public int Id { get; set; }
        public string Hash { get; set; }

        /// <summary>
        /// Object to Target Align 시 사용하자.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ZoneID 
        /// History 발생 작업 구역을 나타냄.
        /// </summary>
        public eExecuteZone Zone { get; set; }

        /// <summary>
        /// GrabNo 같은 Camera 에서 1차 , 2차를 구별하기 위해 사용.
        /// </summary>
        //public eGrabQuarter GrabNo { get; set; }

        public XYT Result { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsSuccess { get; set; }

        public AlignHistory()
        {
            CreateDate = DateTime.Now;
        }
    }
}
