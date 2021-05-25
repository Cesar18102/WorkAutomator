using System.Collections.Generic;

using WorkAutomatorLogic.Models.Roles;

namespace WorkAutomatorLogic.Models
{
    public class CompanyModel : ModelBase
    {
        public string Name { get; set; }
        public string PlanImageUrl { get; set; }

        public AccountModel Owner { get; set; }
        public ICollection<AccountModel> Members { get; set; }
        public ICollection<PlanPointModel> CompanyPlanUniquePoints { get; set; }
        public ICollection<ManufactoryModel> Manufactories { get; set; }
        public ICollection<RoleModel> Roles { get; set; }

        //public ICollection<DetectorPrefabEntity> DetectorPrefabs { get; set; }
        //public virtual ICollection<PipelineItemPrefabEntity> PipelineItemPrefabs { get; set; }
        //public virtual ICollection<PipelineEntity> Pipelines { get; set; }
        //public virtual ICollection<ResourceEntity> Resources { get; set; }
        //public virtual ICollection<StorageCellPrefabEntity> StorageCellPrefabs { get; set; }
        //public virtual ICollection<TaskEntity> Tasks { get; set; }
        //public virtual ICollection<UnitEntity> Units { get; set; }
    }
}
