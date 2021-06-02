using System.Collections.Generic;

using Newtonsoft.Json;

using WorkAutomatorLogic.Models.Prefabs;

namespace WorkAutomatorLogic.Models.Pipeline
{
    public class PipelineItemModel : IdModel
    {
        [JsonProperty("prefab")]
        public PipelineItemPrefabModel Prefab { get; set; }

        [JsonProperty("settings_values")]
        public ICollection<PipelineItemSettingsValueModel> SettingsValues { get; set; }

        [JsonProperty("detectors")]
        public ICollection<DetectorModel> Detectors { get; set; }

        [JsonProperty("manufactory_id")]
        public int? ManufactoryId { get; set; }

        [JsonProperty("x")]
        public double? x { get; set; }

        [JsonProperty("y")]
        public double? y { get; set; }
    }
}
