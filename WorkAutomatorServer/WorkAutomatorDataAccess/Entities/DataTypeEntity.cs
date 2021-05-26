namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data_type")]
    public partial class DataTypeEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DataTypeEntity()
        {
            detector_data_prefab = new HashSet<DetectorDataPrefabEntity>();
            detector_settings_prefab = new HashSet<DetectorSettingsPrefabEntity>();
            pipeline_item_settings_prefab = new HashSet<PipelineItemSettingsPrefabEntity>();
        }

        
        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorDataPrefabEntity> detector_data_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorSettingsPrefabEntity> detector_settings_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemSettingsPrefabEntity> pipeline_item_settings_prefab { get; set; }
    }
}
