namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StorageCellPrefabEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StorageCellPrefabEntity()
        {
            storage_cell = new HashSet<StorageCellEntity>();
        }

        
        public int company_id { get; set; }

        [Required]
        [StringLength(1024)]
        public string image_url { get; set; }

        public double input_x { get; set; }

        public double input_y { get; set; }

        public double output_x { get; set; }

        public double output_y { get; set; }

        public virtual CompanyEntity company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageCellEntity> storage_cell { get; set; }
    }
}
