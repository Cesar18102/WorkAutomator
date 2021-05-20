namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("resource")]
    public partial class ResourceEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ResourceEntity()
        {
            resource_storage_cell = new HashSet<ResourceStorageCellEntity>();
        }

        
        public int company_id { get; set; }

        public int unit_id { get; set; }

        [Required]
        [StringLength(1024)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [StringLength(1024)]
        public string image_url { get; set; }

        public virtual CompanyEntity Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResourceStorageCellEntity> resource_storage_cell { get; set; }

        public virtual UnitEntity Unit { get; set; }
    }
}
