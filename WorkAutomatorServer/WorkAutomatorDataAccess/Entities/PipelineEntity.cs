namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pipeline")]
    public partial class PipelineEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PipelineEntity()
        {
            PipelineItems = new HashSet<PipelineItemEntity>();
            StorageCells = new HashSet<StorageCellEntity>();
        }

        
        public int company_id { get; set; }

        public virtual CompanyEntity Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PipelineItemEntity> PipelineItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageCellEntity> StorageCells { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return company_id == companyId;
        }
    }
}
