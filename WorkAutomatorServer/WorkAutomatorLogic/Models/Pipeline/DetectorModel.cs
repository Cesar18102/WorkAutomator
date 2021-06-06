using System.Collections.Generic;

using Newtonsoft.Json;

using WorkAutomatorLogic.Models.Prefabs;

namespace WorkAutomatorLogic.Models.Pipeline
{
    public class DetectorModel : IdModel
    {
        [JsonProperty("pipeline_item_id")]
        public int? PipelineItemId { get; set; }

        [JsonProperty("prefab")]
        public DetectorPrefabModel Prefab { get; set; }

        [JsonProperty("tracked_detector_fault_prefabs")]
        public ICollection<DetectorFaultPrefabModel> TrackedDetectorFaults { get; set; }

        [JsonProperty("settings_values")]
        public ICollection<DetectorSettingsValueModel> SettingsValues { get; set; }
    }
}
