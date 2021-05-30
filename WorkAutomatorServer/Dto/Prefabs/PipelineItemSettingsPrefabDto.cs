using Attributes;
using Constants;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace Dto.Prefabs
{
    public class PipelineItemSettingsPrefabDto : IdDto
    {
        [ObjectId(DbTable.PipelineItemSettingsPrefab)]
        [JsonIgnore]
        public int? PipelineItemSettingsPrefabId => Id;

        [ObjectId(DbTable.DataType)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty("data_type_id")]
        public int? OptionDataTypeId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty("option_name")]
        public string OptionName { get; set; }

        [JsonProperty("description")]
        public string OptionDescription { get; set; }
    }
}
