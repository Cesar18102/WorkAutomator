using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.DetectorData
{
    public class DetectorDataItemDto : IdDto
    {
        [ObjectId(DbTable.DetectorDataPrefab)]
        [JsonIgnore]
        public int? DetectorDataPrefabId => Id;

        [Base64]
        [JsonProperty("data_base64")]
        public string DataBase64 { get; set; }
    }
}
