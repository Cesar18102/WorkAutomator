using Newtonsoft.Json;

using System.Collections.Generic;

using WorkAutomatorLogic.Models.Event;
using WorkAutomatorLogic.Models.Roles;

namespace WorkAutomatorLogic.Models
{
    public class WorkerModel : AccountModel
    {
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("roles")]
        public ICollection<RoleModel> Roles { get; set; }

        [JsonProperty("subs")]
        public ICollection<AccountModel> Subs { get; set; }

        [JsonProperty("bosses")]
        public ICollection<AccountModel> Bosses { get; set; }

        [JsonProperty("assigned_tasks")]
        public ICollection<TaskModel> AssignedTasks { get; set; }

        [JsonProperty("tasks_to_review")]
        public ICollection<TaskModel> TasksToReview { get; set; }

        [JsonProperty("check_point_events")]
        public ICollection<CheckPointEventModel> CheckPointEvents { get; set; }

        [JsonProperty("detector_interaction_events")]
        public ICollection<DetectorInteractionEventModel> DetectorInteractionEvents { get; set; }

        [JsonProperty("enter_leave_point_events")]
        public ICollection<EnterLeavePointEventModel> EnterLeavePointEvents { get; set; }

        [JsonProperty("pipeline_item_interaction_events")]
        public ICollection<PipelineItemInteractionEventModel> PipelineItemInteractionEvents { get; set; }

        [JsonProperty("storage_cell_events")]
        public ICollection<StorageCellEventModel> StorageCellEvents { get; set; }
    }
}
