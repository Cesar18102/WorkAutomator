using Attributes;
using Constants;

using Newtonsoft.Json;

namespace Dto.Tasks
{
    public class TaskDto : IdDto
    {
        [ObjectId(DbTable.Task)]
        [JsonIgnore]
        public int? TaskId => Id;

        [CompanyId]
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
