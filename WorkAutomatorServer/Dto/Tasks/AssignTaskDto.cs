using Attributes;
using Constants;

using Newtonsoft.Json;

namespace Dto.Tasks
{
    public class AssignTaskDto : IdDto
    {
        [ObjectId(DbTable.Task)]
        [JsonIgnore]
        public int? TaskId => Id;

        [ObjectId(DbTable.Account)]
        [JsonProperty("assignee_account_id")]
        public int? AssigneeAccountId { get; set; }

        [ObjectId(DbTable.Account)]
        [JsonProperty("reviewer_account_id")]
        public int? ReviewerAccountId { get; set; }
    }
}
