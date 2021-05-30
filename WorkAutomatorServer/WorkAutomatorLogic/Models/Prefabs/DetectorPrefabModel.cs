using System.Collections.Generic;

using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Prefabs
{
    public class DetectorPrefabModel : IdModel
    {
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("detector_data_prefabs")]
        public ICollection<DetectorDataPrefabModel> DetectorDataPrefabs { get; set; }

        [JsonProperty("detector_fault_prefabs")]
        public ICollection<DetectorFaultPrefabModel> DetectorFaultPrefabs { get; set; }

        [JsonProperty("detector_settings_prefabs")]
        public ICollection<DetectorSettingsPrefabModel> DetectorSettingsPrefabs { get; set; }
    }
}
