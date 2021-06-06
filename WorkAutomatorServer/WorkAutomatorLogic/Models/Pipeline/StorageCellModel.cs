using Newtonsoft.Json;

using WorkAutomatorLogic.Models.Prefabs;

namespace WorkAutomatorLogic.Models.Pipeline
{
    public class StorageCellModel : IdModel
    {
        [JsonProperty("prefab")]
        public StorageCellPrefabModel Prefab { get; set; }

        [JsonProperty("pipeline_id")]
        public int? PipelineId { get; set; }

        [JsonProperty("manufactory_id")]
        public int? ManufactoryId { get; set; }

        [JsonProperty("x")]
        public double? x { get; set; }

        [JsonProperty("y")]
        public double? y { get; set; }
    }
}
