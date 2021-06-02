using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.DetectorData
{
    public class DetectorDataDto : IdDto
    {
        [ObjectId(DbTable.Detector)]
        [JsonIgnore]
        public int? DetectorId => Id;

        [JsonProperty("data")]
        public DetectorDataItemDto[] Data { get; set; }
    }
}
