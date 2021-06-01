using System.Collections.Generic;

using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Pipeline
{
    public class PipelineItemConnectionModel
    {
        [JsonProperty("pipeline_item")]
        public PipelineItemModel PipelineItem { get; set; }

        [JsonProperty("input_pipeline_items")]
        public ICollection<PipelineItemModel> InputPipelineItems { get; set; }

        [JsonProperty("output_pipeline_items")]
        public ICollection<PipelineItemModel> OutputPipelineItems { get; set; }

        [JsonProperty("input_storage_cells")]
        public ICollection<StorageCellModel> InputStorageCells { get; set; }

        [JsonProperty("output_storage_cells")]
        public ICollection<StorageCellModel> OutputStorageCells { get; set; }
    }
}
