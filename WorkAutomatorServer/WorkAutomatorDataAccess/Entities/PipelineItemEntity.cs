namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PipelineItemEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PipelineItemEntity()
        {
            detector = new HashSet<DetectorEntity>();
            pipeline_item_interaction_event = new HashSet<PipelineItemInteractionEventEntity>();
            pipeline_item_settings_value = new HashSet<PipelineItemSettingsValueEntity>();
            pipeline_item_connection = new HashSet<PipelineItemConnectionEntity>();
            pipeline_item_connection1 = new HashSet<PipelineItemConnectionEntity>();
            pipeline_item_storage_connection = new HashSet<PipelineItemStorageConnectionEntity>();
            PermissionsGranted = new HashSet<RoleEntity>();
        }

        
        public int pipeline_id { get; set; }

        public int pipeline_item_prefab_id { get; set; }

        public int manufactory_id { get; set; }

        public double x { get; set; }

        public double y { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorEntity> detector { get; set; }

        public virtual ManufactoryEntity manufactory { get; set; }

        public virtual PipelineEntity pipeline { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemInteractionEventEntity> pipeline_item_interaction_event { get; set; }

        public virtual PipelineItemPrefabEntity pipeline_item_prefab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemSettingsValueEntity> pipeline_item_settings_value { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemConnectionEntity> pipeline_item_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemConnectionEntity> pipeline_item_connection1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemStorageConnectionEntity> pipeline_item_storage_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> PermissionsGranted { get; set; }
    }
}
