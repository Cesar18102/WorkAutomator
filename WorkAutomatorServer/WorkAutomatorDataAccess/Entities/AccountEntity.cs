namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("account")]
    public partial class AccountEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccountEntity()
        {
            Roles = new HashSet<RoleEntity>();
            Bosses = new HashSet<AccountEntity>();
            Subs = new HashSet<AccountEntity>();
            CheckPointEvents = new HashSet<CheckPointEventEntity>();
            DetectorInteractionEvents = new HashSet<DetectorInteractionEventEntity>();
            EnterLeavePointEvents = new HashSet<EnterLeavePointEventEntity>();
            PipelineItemInteractionEvents = new HashSet<PipelineItemInteractionEventEntity>();
            StorageCellEvents = new HashSet<StorageCellEventEntity>();
            AssignedTasks = new HashSet<TaskEntity>();
            TasksToReview = new HashSet<TaskEntity>();
        }

        public int? company_id { get; set; }

        [Required]
        [StringLength(256)]
        [Index(IsUnique = true)]
        public string login { get; set; }

        [Required]
        [StringLength(256)]
        public string password { get; set; }

        [Required]
        [StringLength(256)]
        public string first_name { get; set; }

        [Required]
        [StringLength(256)]
        public string last_name { get; set; }

        [Required]
        public bool is_superadmin { get; set; }

        public virtual CompanyEntity Company { get; set; }
        public virtual CompanyEntity OwnedCompany { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> Roles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountEntity> Subs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountEntity> Bosses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckPointEventEntity> CheckPointEvents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorInteractionEventEntity> DetectorInteractionEvents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnterLeavePointEventEntity> EnterLeavePointEvents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemInteractionEventEntity> PipelineItemInteractionEvents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageCellEventEntity> StorageCellEvents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskEntity> AssignedTasks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskEntity> TasksToReview { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return company_id == companyId;
        }
    }
}
