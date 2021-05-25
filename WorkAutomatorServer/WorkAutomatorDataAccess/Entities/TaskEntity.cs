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

        public int assignee_account_id { get; set; }

        public int reviewer_account_id { get; set; }

        public virtual AccountEntity Assignee { get; set; }

        public virtual AccountEntity Reviewer { get; set; }

        public virtual CompanyEntity company { get; set; }
    }
}
