using System;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.DetectorData
{
    public class GetDetectorDataDto : IdDto
    {
        [ObjectId(DbTable.Detector)]
        [JsonIgnore]
        public int? DetectorId => Id;

        [JsonProperty("date_since")]
        public DateTime DateSince { get; set; }

        [JsonProperty("date_until")]
        public DateTime DateUntil { get; set; }
    }
}
