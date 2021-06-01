using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class PipelineItemPlacementDto : IdDto
    {
        [ObjectId(DbTable.PipelineItem)]
        [JsonIgnore]
        public int? PipelineItemId => Id;

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("x")]
        public double? X { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("y")]
        public double? Y { get; set; }
    }
}
