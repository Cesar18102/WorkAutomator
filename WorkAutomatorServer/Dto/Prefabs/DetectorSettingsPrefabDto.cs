using System.ComponentModel.DataAnnotations;

using Attributes;
using Constants;

using Newtonsoft.Json;

namespace Dto.Prefabs
{
    public class DetectorSettingsPrefabDto : IdDto
    {
        [ObjectId(DbTable.DetectorSettingsPrefab)]
        [JsonIgnore]
        public int? DetectorSettingsPrefabId => Id;

        [Required(AllowEmptyStrings = false)]
        [ObjectId(DbTable.DataType)]
        [JsonProperty("data_type_id")]
        public int? OptionDataTypeId { get; set; }

        [JsonProperty("option_name")]
        public string OptionName { get; set; }

        [JsonProperty("option_description")]
        public string OptionDescription { get; set; }
    }
}
