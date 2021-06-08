using Newtonsoft.Json;
using System;

namespace WorkAutomatorLogic.Models.Event
{
    public class CheckPointEventModel : IdModel
    {
        [JsonProperty("check_point")]
        public CheckPointModel CheckPoint { get; set; }

        [JsonProperty("timespan")]
        public DateTime Timespan { get; set; }

        [JsonProperty("is_direct")]
        public bool IsDirect { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }
    }
}
