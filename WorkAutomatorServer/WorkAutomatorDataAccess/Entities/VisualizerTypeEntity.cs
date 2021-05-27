namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("visualizer_type")]
    public partial class VisualizerTypeEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VisualizerTypeEntity()
        {
            detector_data_prefab = new HashSet<DetectorDataPrefabEntity>();
        }

        
        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorDataPrefabEntity> detector_data_prefab { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return false;
        }
    }
}
