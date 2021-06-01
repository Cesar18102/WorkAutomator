using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class StorageCellPlacementDto : IdDto
    {
        [ObjectId(DbTable.StorageCell)]
        [JsonIgnore]
        public int? StorageCellId => Id;

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("x")]
        public double? X { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("y")]
        public double? Y { get; set; }
    }
}
