﻿using System.Collections.Generic;

namespace WorkAutomatorLogic.Models
{
    public class WorkerModel : AccountModel
    {
        public int? CompanyId { get; set; }

        public ICollection<RoleModel> Roles { get; set; }
        public ICollection<AccountModel> Subs { get; set; }
        public ICollection<AccountModel> Bosses { get; set; }
        /*public ICollection<TaskEntity> AssignedTasks { get; set; }
        public ICollection<TaskEntity> TasksToReview { get; set; }*/
    }
}
