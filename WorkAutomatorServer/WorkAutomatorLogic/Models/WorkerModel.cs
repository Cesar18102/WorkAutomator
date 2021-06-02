using System.Collections.Generic;

using WorkAutomatorLogic.Models.Roles;

namespace WorkAutomatorLogic.Models
{
    public class WorkerModel : AccountModel
    {
        public int? CompanyId { get; set; }

        public ICollection<RoleModel> Roles { get; set; }
        public ICollection<AccountModel> Subs { get; set; }
        public ICollection<AccountModel> Bosses { get; set; }
        public ICollection<TaskModel> AssignedTasks { get; set; }
        public ICollection<TaskModel> TasksToReview { get; set; }
    }
}
