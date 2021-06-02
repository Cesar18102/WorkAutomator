namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("task")]
    public partial class TaskEntity : IdEntity
    {
        
        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        public int company_id { get; set; }

        public int? creator_account_id { get; set; }
        public int? assignee_account_id { get; set; }
        public int? reviewer_account_id { get; set; }

        public bool is_done { get; set; }
        public bool is_reviewed { get; set; }

        public virtual DetectorFaultEventEntity AssociatedFault { get; set; }

        public virtual AccountEntity Assignee { get; set; }
        public virtual AccountEntity Creator { get; set; }
        public virtual AccountEntity Reviewer { get; set; }

        public virtual CompanyEntity company { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return company_id == companyId;
        }
    }
}
