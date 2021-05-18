namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CheckPointEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CheckPointEntity()
        {
            check_point_event = new HashSet<CheckPointEventEntity>();
        }
        
        public int company_plan_unique_point1_id { get; set; }

        public int company_plan_unique_point2_id { get; set; }

        public int manufactory1_id { get; set; }

        public int manufactory2_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckPointEventEntity> check_point_event { get; set; }

        public virtual CompanyPlanUniquePointEntity company_plan_unique_point1 { get; set; }

        public virtual CompanyPlanUniquePointEntity company_plan_unique_point2 { get; set; }

        public virtual ManufactoryEntity manufactory1 { get; set; }

        public virtual ManufactoryEntity manufactory2 { get; set; }
    }
}
