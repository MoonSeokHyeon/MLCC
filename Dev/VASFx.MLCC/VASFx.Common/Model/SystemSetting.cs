using System;
using VASFx.Common.Interface;

namespace VASFx.Common.Model
{
    public class SystemSetting : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime EditTime { get; set; }
        public string Value { get; set; }
    }
}

 
