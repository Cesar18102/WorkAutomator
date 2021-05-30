using System.Collections.Generic;

using Newtonsoft.Json;

using Attributes;
using Constants;

namespace Dto.Pipeline
{
    public class PipelineDto : IdDto
    {
        [ObjectId(DbTable.Pipeline)]
        [JsonIgnore]
        public int? PipelineId => Id;

        [CompanyId]
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("connections")]
        public ICollection<PipelineConnectionDto> Connections { get; set; }

        [JsonProperty("pipeline_item_placemetns")]
        public ICollection<PipelineItemPlacementDto> PipelineItemPlacements { get; set; }

        [JsonProperty("storage_cell_placemetns")]
        public ICollection<StorageCellPlacementDto> StorageCellPlacements { get; set; }
    }
}
