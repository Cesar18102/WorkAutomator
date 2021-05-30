using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class SetupDetectorDto : DtoBase
    {
        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.PipelineItem)]
        [JsonProperty("pipeline_item_id")]
        public int? PipelineItemId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.Detector)]
        [JsonProperty("detector_id")]
        public int? DetectorId { get; set; }
    }
}
