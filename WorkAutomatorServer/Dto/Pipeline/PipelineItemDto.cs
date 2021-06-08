using Newtonsoft.Json;

using Constants;
using Attributes;

namespace Dto.Pipeline
{
    public class PipelineItemDto : IdDto
    {
        [ObjectId(DbTable.PipelineItem)]
        [JsonIgnore]
        public int? PipelineItemId => Id;

        [ObjectId(DbTable.PipelineItemPrefab)]
        [JsonProperty("prefab_id")]
        public int? PrefabId { get; set; }

        [JsonProperty("settings_values")]
        public PipelineItemSettingsValueDto[] SettingsValues { get; set; }
    }
}
