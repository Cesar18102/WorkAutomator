namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EnterLeavePointEntity : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnterLeavePointEntity()
        {
            enter_leave_point_event = new HashSet<EnterLeavePointEventEntity>();
        }

        
        public int company_plan_unique_point1_id { get; set; }

        public int company_plan_unique_point2_id { get; set; }

        public virtual CompanyPlanUniquePointEntity company_plan_unique_point1 { get; set; }

        public virtual CompanyPlanUniquePointEntity company_plan_unique_point2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnterLeavePointEventEntity> enter_leave_point_event { get; set; }
    }
}
