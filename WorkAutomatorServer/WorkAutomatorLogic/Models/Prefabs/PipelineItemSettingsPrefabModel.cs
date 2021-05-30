using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Prefabs
{
    public class PipelineItemSettingsPrefabModel : IdModel
    {
        [JsonProperty("data_type")]
        public DataTypeModel OptionDataType { get; set; }

        [JsonProperty("option_name")]
        public string OptionName { get; set; }

        [JsonProperty("description")]
        public string OptionDescription { get; set; }
    }
}
