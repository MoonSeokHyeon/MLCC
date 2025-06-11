using System;
using System.Collections.Generic;
using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class ResultMLCCInspection
    {
        public DateTime CreateTime { get; set; }
        public int ID { get; set; }
        public List<ResultMLCCItem> ResultMLCCItems { get; set; } = new List<ResultMLCCItem>();
        public object Graphics { get; set; }
        public object Image { get; set; }
        public eVisionAlarm Alarm { get; set; }

        public ResultMLCCInspection(int id)
        {
            this.ID = id;
            this.CreateTime = DateTime.Now;
        }
    }

    public class ResultMLCCItem
    {
        public int ID { get; set; }
        public eMesh MeshID{ get; set; }
        public double Area{ get; set; }
        public bool IsExistence { get; set; } = false;
        public bool IsOverlap { get; set; } = false;
        public ResultMLCCItem(int id)
        {
            this.ID = id;
        }
    }
}
