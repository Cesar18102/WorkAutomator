using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class PipelineItemSettingsValueDto : IdDto
    {
        [ObjectId(DbTable.PipelineItemSettingsValue)]
        [JsonIgnore]
        public int? PipelineItemSettingValueId => Id;

        [ObjectId(DbTable.PipelineItemSettingsPrefab)]
        [JsonProperty("prefab_id")]
        public int? PrefabId { get; set; }

        [Base64]
        [JsonProperty("value_base64")]
        public string ValueBase64 { get; set; }
    }
}
