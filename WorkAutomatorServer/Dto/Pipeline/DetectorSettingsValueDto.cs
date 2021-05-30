using Attributes;

using Constants;
using Newtonsoft.Json;

namespace Dto.Pipeline
{
    public class DetectorSettingsValueDto : IdDto
    {
        [ObjectId(DbTable.DetectorSettingsValue)]
        [JsonIgnore]
        public int? DetectorSettingValueId => Id;

        [ObjectId(DbTable.DetectorSettingsPrefab)]
        [JsonProperty("prefab_id")]
        public int? PrefabId { get; set; }

        [JsonProperty("value_base64")]
        public string ValueBase64 { get; set; }
    }
}
