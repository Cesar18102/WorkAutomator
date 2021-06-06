using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Prefabs
{
    public class DetectorFaultPrefabModel : IdModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fault_condition")]
        public string FaultCondition { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }
}
