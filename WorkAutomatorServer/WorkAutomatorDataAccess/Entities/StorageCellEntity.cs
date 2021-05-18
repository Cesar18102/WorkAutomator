namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StorageCellEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StorageCellEntity()
        {
            pipeline_item_storage_connection = new HashSet<PipelineItemStorageConnectionEntity>();
            resource_storage_cell = new HashSet<ResourceStorageCellEntity>();
            PermissionsGranted = new HashSet<RoleEntity>();
            storage_cell_event = new HashSet<StorageCellEventEntity>();
        }

        
        public int manufactory_id { get; set; }

        public int storage_cell_prefab_id { get; set; }

        public double x { get; set; }

        public double y { get; set; }

        public virtual ManufactoryEntity manufactory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemStorageConnectionEntity> pipeline_item_storage_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResourceStorageCellEntity> resource_storage_cell { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> PermissionsGranted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageCellEventEntity> storage_cell_event { get; set; }

        public virtual StorageCellPrefabEntity storage_cell_prefab { get; set; }
    }
}
