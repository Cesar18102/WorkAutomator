namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector_fault_prefab")]
    public partial class DetectorFaultPrefabEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetectorFaultPrefabEntity()
        {
            Detectors = new HashSet<DetectorEntity>();
            DetectorsFaultEvents = new HashSet<DetectorFaultEventEntity>();
        }

        
        public int detector_prefab_id { get; set; }

        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        public string fault_condition { get; set; }

        public virtual DetectorPrefabEntity detector_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorEntity> Detectors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorFaultEventEntity> DetectorsFaultEvents { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return detector_prefab.IsOwnedByCompany(companyId);
        }
    }
}
