using System.Collections.Generic;

using Newtonsoft.Json;

using WorkAutomatorLogic.Models.Pipeline;

namespace WorkAutomatorLogic.Models.DetectorData
{
    public class DetectorDataModel
    {
        [JsonProperty("detector")]
        public DetectorModel Detector { get; set; }

        [JsonProperty("data")]
        public ICollection<DetectorDataItemModel> Data { get; set; }
    }
}
