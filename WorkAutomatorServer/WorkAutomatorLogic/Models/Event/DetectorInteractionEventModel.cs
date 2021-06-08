using Newtonsoft.Json;
using System;
using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.Models.Event
{
    public class DetectorInteractionEventModel : IdModel
    {
        [JsonProperty("detector")]
        public DetectorModel Detector { get; set; }

        [JsonProperty("timespan")]
        public DateTime Timespan { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }
    }
}
