using System.Collections.Generic;

using Newtonsoft.Json;
using WorkAutomatorLogic.Models.Prefabs;
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

        [JsonProperty("check_points")]
        public ICollection<CheckPointModel> CheckPoints { get; set; }

        [JsonProperty("enter_leave_points")]
        public ICollection<EnterLeavePointModel> EnterLeavePoints { get; set; }

        [JsonProperty("pipeline_item_prefabs")]
        public ICollection<PipelineItemPrefabModel> PipelineItemPrefabs { get; set; }

        [JsonProperty("storage_cell_prefabs")]
        public ICollection<StorageCellPrefabModel> StorageCellPrefabs { get; set; }

        [JsonProperty("detector_prefabs")]
        public ICollection<DetectorPrefabModel> DetectorPrefabs { get; set; }


        //public virtual ICollection<ResourceEntity> Resources { get; set; }
        //public virtual ICollection<TaskEntity> Tasks { get; set; }
        //public virtual ICollection<UnitEntity> Units { get; set; }
    }
}
