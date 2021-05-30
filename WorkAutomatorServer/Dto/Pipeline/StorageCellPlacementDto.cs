using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class StorageCellPlacementDto : IdDto
    {
        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.StorageCell)]
        [JsonProperty("storage_cell_id")]
        public int? StorageCellId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("x")]
        public float? X { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("y")]
        public float? Y { get; set; }
    }
}
