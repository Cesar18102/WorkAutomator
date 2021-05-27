namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector_fault")]
    public partial class DetectorFaultEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetectorFaultEntity()
        {
            detector_fault_events = new HashSet<DetectorFaultEventEntity>();
        }

        
        public int detector_id { get; set; }

        public int detector_fault_prefab_id { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public virtual DetectorEntity detector { get; set; }

        public virtual DetectorFaultPrefabEntity detector_fault_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorFaultEventEntity> detector_fault_events { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return detector.IsOwnedByCompany(companyId);
        }
    }
}
