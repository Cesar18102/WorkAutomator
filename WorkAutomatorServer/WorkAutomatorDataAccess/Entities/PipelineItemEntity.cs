namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pipeline_item")]
    public partial class PipelineItemEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PipelineItemEntity()
        {
            Detectors = new HashSet<DetectorEntity>();
            pipeline_item_interaction_event = new HashSet<PipelineItemInteractionEventEntity>();
            PipelineItemSettingsValues = new HashSet<PipelineItemSettingsValueEntity>();
            InputPipelineItemConnections = new HashSet<PipelineItemConnectionEntity>();
            OutputPipelineItemConnections = new HashSet<PipelineItemConnectionEntity>();
            PipelineItemStorageConnections = new HashSet<PipelineItemStorageConnectionEntity>();
            PermissionsGranted = new HashSet<RoleEntity>();
        }

        
        public int pipeline_id { get; set; }

        public int pipeline_item_prefab_id { get; set; }

        public int manufactory_id { get; set; }

        public double x { get; set; }

        public double y { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorEntity> Detectors { get; set; }

        public virtual ManufactoryEntity Manufactory { get; set; }

        public virtual PipelineEntity pipeline { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemInteractionEventEntity> pipeline_item_interaction_event { get; set; }

        public virtual PipelineItemPrefabEntity PipelineItemPrefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemSettingsValueEntity> PipelineItemSettingsValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemConnectionEntity> InputPipelineItemConnections { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemConnectionEntity> OutputPipelineItemConnections { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemStorageConnectionEntity> PipelineItemStorageConnections { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> PermissionsGranted { get; set; }
    }
}
