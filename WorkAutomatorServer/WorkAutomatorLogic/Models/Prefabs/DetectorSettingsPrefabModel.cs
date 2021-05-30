using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Prefabs
{
    public class DetectorSettingsPrefabModel : IdModel
    {
        [JsonProperty("option_data_type_id")]
        public  DataTypeModel OptionDataType { get; set; }

        [JsonProperty("option_name")]
        public string OptionName { get; set; }

        [JsonProperty("option_description")]
        public string OptionDescription { get; set; }
    }
}
