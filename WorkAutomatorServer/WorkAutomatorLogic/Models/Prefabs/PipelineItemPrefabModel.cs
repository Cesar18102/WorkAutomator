using System.Collections.Generic;

using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Prefabs
{
    public class PipelineItemPrefabModel : ItemPrefabBaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("settings_prefabs")]
        public ICollection<PipelineItemSettingsPrefabModel> PipelineItemSettingsPrefabs { get; set; }
    }
}
