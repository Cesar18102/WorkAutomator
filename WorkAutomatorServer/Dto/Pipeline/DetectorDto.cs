using System.Collections.Generic;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class DetectorDto : IdDto
    {
        [ObjectId(DbTable.Detector)]
        [JsonIgnore]
        public int? DetectorId => Id;

        [ObjectId(DbTable.DetectorPrefab)]
        [JsonProperty("prefab_id")]
        public int? PrefabId { get; set; }

        [JsonProperty("tracked_detector_fault_ids")]
        public ICollection<DetectorFaultDto> TrackedDetectorFaults { get; set; }

        [JsonProperty("settings_values")]
        public ICollection<DetectorSettingsValueDto> SettingsValues { get; set; }
    }
}
