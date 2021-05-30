using System.Collections.Generic;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Prefabs
{
    public class DetectorPrefabDto : IdDto
    {
        [ObjectId(DbTable.DetectorPrefab)]
        [JsonIgnore]
        public int? DetectorPrefabId => Id;

        [CompanyId]
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("detector_data_prefabs")]
        public ICollection<DetectorDataPrefabDto> DetectorDataPrefabs { get; set; }

        [JsonProperty("detector_fault_prefabs")]
        public ICollection<DetectorFaultPrefabDto> DetectorFaultPrefabs { get; set; }

        [JsonProperty("detector_settings_prefabs")]
        public ICollection<DetectorSettingsPrefabDto> DetectorSettingsPrefabs { get; set; }
    }
}
