﻿using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class XlsW
    {
        public int Addr { get; set; }
        public string TagName { get; set; }
        public int Point { get; set; }
        public string SubText { get; set; }
        public string Format { get; set; }
        public int MultipleV { get; set; }
        public string MultipleFormat { get; set; }
        public bool Watch { get; set; }
        public int SubNo { get; set; }
        public int CallbackOrder { get; set; }
        public ePlcKind Kind { get; set; }
        public string Comment { get; set; }
        public bool BitType { get; set; }
    }
}
