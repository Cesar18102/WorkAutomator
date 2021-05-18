namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetectorPrefabEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetectorPrefabEntity()
        {
            detector = new HashSet<DetectorEntity>();
            detector_data_prefab = new HashSet<DetectorDataPrefabEntity>();
            detector_fault_prefab = new HashSet<DetectorFaultPrefabEntity>();
            detector_settings_prefab = new HashSet<DetectorSettingsPrefabEntity>();
        }

        
        public int company_id { get; set; }

        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [Required]
        [StringLength(1024)]
        public string image_url { get; set; }

        public virtual CompanyEntity company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorEntity> detector { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorDataPrefabEntity> detector_data_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorFaultPrefabEntity> detector_fault_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorSettingsPrefabEntity> detector_settings_prefab { get; set; }
    }
}
