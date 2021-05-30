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
        [JsonProperty("pipeline_item_settings_prefab_id")]
        public int? PipelineItemSettingsPrefabId { get; set; }

        [JsonProperty("value_base64")]
        public string ValueBase64 { get; set; }
    }
}
