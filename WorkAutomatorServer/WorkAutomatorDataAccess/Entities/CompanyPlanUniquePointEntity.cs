namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("company_plan_unique_point")]
    public partial class CompanyPlanUniquePointEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyPlanUniquePointEntity()
        {
            CheckPoints = new HashSet<CheckPointEntity>();
            EnterLeavePoints = new HashSet<EnterLeavePointEntity>();
            ManufactoryPlanPoints = new HashSet<ManufactoryPlanPointEntity>();
        }

        
        public int company_id { get; set; }

        public double x { get; set; }

        public double y { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckPointEntity> CheckPoints { get; set; }

        public virtual CompanyEntity company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnterLeavePointEntity> EnterLeavePoints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManufactoryPlanPointEntity> ManufactoryPlanPoints { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return company_id == companyId;
        }
    }
}
