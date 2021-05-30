using Newtonsoft.Json;

using WorkAutomatorLogic.Models.Prefabs;

namespace WorkAutomatorLogic.Models.Pipeline
{
    public class DetectorSettingsValueModel : IdModel
    {
        [JsonProperty("prefab")]
        public DetectorSettingsPrefabModel Prefab { get; set; }

        [JsonProperty("value_base64")]
        public string ValueBase64 { get; set; }
    }
}
