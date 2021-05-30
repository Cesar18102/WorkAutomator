using System.ComponentModel.DataAnnotations;

using Attributes;
using Constants;

using Newtonsoft.Json;

namespace Dto.Prefabs
{
    public class DetectorDataPrefabDto : IdDto
    {
        [ObjectId(DbTable.DetectorDataPrefab)]
        [JsonIgnore]
        public int? DetectorDataPrefabId => Id;

        [ObjectId(DbTable.VisualizerType)]
        [JsonProperty("visualizer_type_id")]
        public int? VisualizerTypeId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.DataType)]
        [JsonProperty("field_data_type_id")]
        public int? FieldDataTypeId { get; set; }

        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        [JsonProperty("field_description")]
        public string FieldDescription { get; set; }

        [JsonProperty("argument_name")]
        public string ArgumentName { get; set; }
    }
}
