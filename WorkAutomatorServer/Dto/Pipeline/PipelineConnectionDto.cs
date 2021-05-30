using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class PipelineConnectionDto
    {
        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.PipelineItem)]
        [JsonProperty("pipeline_item_id")]
        public int? PipelineItemId { get; set; }

        [JsonProperty("input_pipeline_items")]
        public ICollection<PipelineItemDto> InputPipelineItems { get; set; }

        [JsonProperty("output_pipeline_items")]
        public ICollection<PipelineItemDto> OutputPipelineItems { get; set; }

        [JsonProperty("input_storage_cells")]
        public ICollection<StorageCellDto> InputStorageCells { get; set; }

        [JsonProperty("output_storage_cells")]
        public ICollection<StorageCellDto> OutputStorageCells { get; set; }
    }
}
