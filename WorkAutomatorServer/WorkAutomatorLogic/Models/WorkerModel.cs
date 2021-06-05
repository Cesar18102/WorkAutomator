using Newtonsoft.Json;
using System.Collections.Generic;

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
    }
}
