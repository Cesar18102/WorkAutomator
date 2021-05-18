namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DbPermissionEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DbPermissionEntity()
        {
            Granted = new HashSet<RoleEntity>();
        }

        
        [Required]
        [StringLength(256)]
        public string table_name { get; set; }

        public int db_permission_type_id { get; set; }

        public virtual DbPermissionTypeEntity db_permission_type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> Granted { get; set; }
    }
}
