namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector")]
    public partial class DetectorEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetectorEntity()
        {
            detector_interaction_event = new HashSet<DetectorInteractionEventEntity>();
            DetectorFaults = new HashSet<DetectorFaultEntity>();
            DetectorDatas = new HashSet<DetectorDataEntity>();
            DetectorSettingsValues = new HashSet<DetectorSettingsValueEntity>();
            PermissionsGranted = new HashSet<RoleEntity>();
        }

        
        public int detector_prefab_id { get; set; }

        public int pipeline_item_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorInteractionEventEntity> detector_interaction_event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorFaultEntity> DetectorFaults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorDataEntity> DetectorDatas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorSettingsValueEntity> DetectorSettingsValues { get; set; }

        public virtual DetectorPrefabEntity DetectorPrefab { get; set; }

        public virtual PipelineItemEntity PipelineItem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> PermissionsGranted { get; set; }
    }
}
