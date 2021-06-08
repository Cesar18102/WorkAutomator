using Newtonsoft.Json;
using System;
using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.Models.Event
{
    public class PipelineItemInteractionEventModel : IdModel
    {
        [JsonProperty("pipeline_item")]
        public PipelineItemModel PipelineItem { get; set; }

        [JsonProperty("timespan")]
        public DateTime Timespan { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }
    }
}
