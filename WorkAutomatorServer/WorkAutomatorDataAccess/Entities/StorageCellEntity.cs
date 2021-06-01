namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("storage_cell")]
    public partial class StorageCellEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StorageCellEntity()
        {
            InputPipelineItems = new HashSet<PipelineItemEntity>();
            OutputPipelineItems = new HashSet<PipelineItemEntity>();
            ResourcesAtStorageCell = new HashSet<ResourceStorageCellEntity>();
            PermissionsGranted = new HashSet<RoleEntity>();
            storage_cell_event = new HashSet<StorageCellEventEntity>();
        }

        public int? pipeline_id { get; set; }

        public int? manufactory_id { get; set; }

        public int storage_cell_prefab_id { get; set; }

        public double? x { get; set; }

        public double? y { get; set; }

        public virtual PipelineEntity pipeline { get; set; }
        public virtual ManufactoryEntity Manufactory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemEntity> InputPipelineItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemEntity> OutputPipelineItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResourceStorageCellEntity> ResourcesAtStorageCell { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> PermissionsGranted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageCellEventEntity> storage_cell_event { get; set; }

        public virtual StorageCellPrefabEntity StorageCellPrefab { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return StorageCellPrefab.IsOwnedByCompany(companyId);
        }
    }
}
