using VASFx.Common.Shared;

namespace VASFx.Common.Model
{
    public class XlsB
    {
        public int Addr { get; set; }
        //public int BitIndex { get; set; }
        public string TagName { get; set; }
        public string SubText { get; set; }
        public int SubNo { get; set; }
        public int CallbackOrder { get; set; }
        public ePlcKind Kind { get; set; }
        public string Comment { get; set; }
        public bool AutoOff { get; set; }
    }
}
