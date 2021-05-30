using System.Collections.Generic;

using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Pipeline
{
    public class PipelineModel : IdModel
    {
        [JsonProperty("pipeline_items")]
        public ICollection<PipelineItemModel> PipelineItems { get; set; }
    }
}
