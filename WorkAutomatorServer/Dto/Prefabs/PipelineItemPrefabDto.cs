using System.Collections.Generic;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Prefabs
{
    public class PipelineItemPrefabDto : ItemPrefabBaseDto
    {
        [ObjectId(DbTable.PipelineItemPrefab)]
        public int? PipelineItemPrefabId => Id; 

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("settings_prefabs")]
        public ICollection<PipelineItemSettingsPrefabDto> SettingsPrefabs { get; set; }
    }
}
