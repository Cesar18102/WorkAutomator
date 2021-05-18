namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector")]
    public partial class DetectorEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetectorEntity()
        {
            detector_interaction_event = new HashSet<DetectorInteractionEventEntity>();
            detector_fault = new HashSet<DetectorFaultEntity>();
            detector_data = new HashSet<DetectorDataEntity>();
            detector_settings_value = new HashSet<DetectorSettingsValueEntity>();
            PermissionsGranted = new HashSet<RoleEntity>();
        }

        
        public int detector_prefab_id { get; set; }

        public int pipeline_item_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorInteractionEventEntity> detector_interaction_event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorFaultEntity> detector_fault { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorDataEntity> detector_data { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorSettingsValueEntity> detector_settings_value { get; set; }

        public virtual DetectorPrefabEntity detector_prefab { get; set; }

        public virtual PipelineItemEntity pipeline_item { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> PermissionsGranted { get; set; }
    }
}
