namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AccountEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccountEntity()
        {
            Roles = new HashSet<RoleEntity>();
            Bosses = new HashSet<AccountEntity>();
            Subs = new HashSet<AccountEntity>();
            check_point_event = new HashSet<CheckPointEventEntity>();
            detector_interaction_event = new HashSet<DetectorInteractionEventEntity>();
            enter_leave_point_event = new HashSet<EnterLeavePointEventEntity>();
            pipeline_item_interaction_event = new HashSet<PipelineItemInteractionEventEntity>();
            storage_cell_event = new HashSet<StorageCellEventEntity>();
            AssignedTasks = new HashSet<TaskEntity>();
            TasksToReview = new HashSet<TaskEntity>();
        }

        
        public int company_id { get; set; }

        [Required]
        [StringLength(256)]
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

        public virtual CompanyEntity company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleEntity> Roles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountEntity> Subs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountEntity> Bosses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckPointEventEntity> check_point_event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual CompanyEntity OwnedCompany { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetectorInteractionEventEntity> detector_interaction_event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnterLeavePointEventEntity> enter_leave_point_event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemInteractionEventEntity> pipeline_item_interaction_event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageCellEventEntity> storage_cell_event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskEntity> AssignedTasks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskEntity> TasksToReview { get; set; }
    }
}
