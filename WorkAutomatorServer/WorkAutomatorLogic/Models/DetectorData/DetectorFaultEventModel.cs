using System;

using Newtonsoft.Json;

using WorkAutomatorLogic.Models.Pipeline;
using WorkAutomatorLogic.Models.Prefabs;

namespace WorkAutomatorLogic.Models.DetectorData
{
    public class DetectorFaultEventModel : IdModel
    {
        [JsonProperty("detector")]
        public DetectorModel Detector { get; set; }

        [JsonProperty("fault")]
        public DetectorFaultPrefabModel Fault { get; set; }

        [JsonProperty("associated_task")]
        public TaskModel AssociatedTask {get; set;}

        [JsonProperty("timespan")]
        public DateTime Timespan { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }

        [JsonProperty("is_fixed")]
        public bool IsFixed { get; set; }
    }
}
