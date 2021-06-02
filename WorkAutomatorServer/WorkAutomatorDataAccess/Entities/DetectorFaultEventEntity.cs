namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector_fault_event")]
    public partial class DetectorFaultEventEntity : EntityBase
    {
        [Key]
        public int associated_task_id { get; set; }

        public int detector_id { get; set; }
        public int detector_fault_prefab_id { get; set; }

        public DateTime timespan { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public bool is_fixed { get; set; }

        public TaskEntity AssociatedTask { get; set; }
        public virtual DetectorEntity detector { get; set; }
        public virtual DetectorFaultPrefabEntity detector_fault_prefab { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return detector.IsOwnedByCompany(companyId);
        }
    }
}
