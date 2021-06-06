using System.Collections.Generic;

using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Prefabs
{
    public class PipelineItemPrefabModel : ItemPrefabBaseModel
    {
        [JsonProperty("settings_prefabs")]
        public ICollection<PipelineItemSettingsPrefabModel> PipelineItemSettingsPrefabs { get; set; }
    }
}
