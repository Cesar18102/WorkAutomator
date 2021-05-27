using System.Collections.Generic;

using Newtonsoft.Json;

using WorkAutomatorLogic.Models.Roles;

namespace WorkAutomatorLogic.Models
{
    public class CompanyModel : IdModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("plan_image_url")]
        public string PlanImageUrl { get; set; }

        [JsonProperty("owner")]
        public AccountModel Owner { get; set; }

        [JsonProperty("members")]
        public ICollection<AccountModel> Members { get; set; }

        [JsonProperty("company_plan_unique_points")]
        public ICollection<CompanyPlanPointModel> CompanyPlanUniquePoints { get; set; }

        [JsonProperty("manufactories")]
        public ICollection<ManufactoryModel> Manufactories { get; set; }

        [JsonProperty("roles")]
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
