using Newtonsoft.Json;
using System.Collections.Generic;

namespace WorkAutomatorLogic.Models
{
    public class ManufactoryModel : IdModel
    {
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("manufactory_plan_points")]
        public ICollection<int> ManufactoryPlanPoints { get; set; }

        /*public ICollection<CheckPointEntity> CheckPoints { get; set; }
        public ICollection<EnterLeavePointEntity> EnterLeavePoints { get; set; }
        public ICollection<PipelineItemEntity> PipelineItems { get; set; }
        public ICollection<StorageCellEntity> StorageCells { get; set; }*/
    }
}
