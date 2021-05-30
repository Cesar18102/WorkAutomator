using WorkAutomatorLogic.Models.Prefabs;

using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Pipeline
{
    public class PipelineItemSettingsValueModel : IdModel
    {
        [JsonProperty("prefab")]
        public PipelineItemSettingsPrefabModel Prefab { get; set; }

        [JsonProperty("value_base64")]
        public string ValueBase64 { get; set; }
    }
}
