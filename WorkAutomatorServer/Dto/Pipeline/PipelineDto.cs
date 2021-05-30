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

        [JsonProperty("pipeline_items")]
        public PipelineItemDto PipelineItems { get; set; }

        [JsonProperty("storage_cells")]
        public StorageCellDto StorageCells { get; set; }
    }
}
