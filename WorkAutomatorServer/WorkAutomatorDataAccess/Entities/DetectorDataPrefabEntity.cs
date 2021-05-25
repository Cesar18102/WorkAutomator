namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetectorDataPrefabEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetectorDataPrefabEntity()
        {
            detector_data = new HashSet<DetectorDataEntity>();
        }

        
        public int detector_prefab_id { get; set; }

        public int? visualizer_type_id { get; set; }

        public int field_data_type_id { get; set; }

        [Required]
        [StringLength(256)]
        public string field_name { get; set; }

        [Column(TypeName = "text")]
        public string field_description { get; set; }

        [StringLength(256)]
        public string argument_name { get; set; }

        public virtual DataTypeEntity DataType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorDataEntity> detector_data { get; set; }

        public virtual DetectorPrefabEntity detector_prefab { get; set; }

        public virtual VisualizerTypeEntity visualizer_type { get; set; }
    }
}
