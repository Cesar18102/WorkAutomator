using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class PipelineItemPlacementDto : IdDto
    {
        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.PipelineItem)]
        [JsonProperty("pipeline_item_id")]
        public int? PipelineItemId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("x")]
        public float? X { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("y")]
        public float? Y { get; set; }
    }
}
