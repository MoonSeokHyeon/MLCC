using System.ComponentModel.DataAnnotations.Schema;
using VASFx.Common.Interface;

namespace VASFx.Common.Model
{
    public class SystemInfo : IEntity
    {
        public int Id { get; set; }

        public string SystemID { get; set; }
        public int? CurrentProductModelId { get; set; }

        [ForeignKey("CurrentProductModelId")]
        public virtual ModelData CurrentModel { get; set; }
    }
}
