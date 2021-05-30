using Newtonsoft.Json;

namespace WorkAutomatorLogic.Models.Prefabs
{
    public class DetectorDataPrefabModel : IdModel
    {
        [JsonProperty("visualizer_type")]
        public VisualizerTypeModel VisualizerType { get; set; }

        [JsonProperty("field_data_type")]
        public DataTypeModel FieldDataType { get; set; }

        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        [JsonProperty("field_description")]
        public string FieldDescription { get; set; }

        [JsonProperty("argument_name")]
        public string ArgumentName { get; set; }
    }
}
