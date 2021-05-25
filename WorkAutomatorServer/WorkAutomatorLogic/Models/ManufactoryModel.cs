using System.Collections.Generic;

namespace WorkAutomatorLogic.Models
{
    public class ManufactoryModel : IdModel
    {
        public int CompanyId { get; set; }
        public virtual ICollection<int> ManufactoryPlanPoints { get; set; }

        /*public ICollection<CheckPointEntity> CheckPoints { get; set; }
        public ICollection<EnterLeavePointEntity> EnterLeavePoints { get; set; }
        public ICollection<PipelineItemEntity> PipelineItems { get; set; }
        public ICollection<StorageCellEntity> StorageCells { get; set; }*/
    }
}
