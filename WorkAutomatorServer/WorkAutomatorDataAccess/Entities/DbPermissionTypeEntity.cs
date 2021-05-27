namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("db_permission_type")]
    public partial class DbPermissionTypeEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DbPermissionTypeEntity()
        {
            db_permission = new HashSet<DbPermissionEntity>();
        }

        
        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DbPermissionEntity> db_permission { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return false;
        }
    }
}
