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

        public int manufactory_id { get; set; }

        [Column("company_plan_unique_point1_id")]
        public int CompanyPlanUniquePoint1Id { get; set; }

        [Column("company_plan_unique_point2_id")]
        public int CompanyPlanUniquePoint2Id { get; set; }

        public virtual ManufactoryEntity Manufactory { get; set; }

        public virtual CompanyPlanUniquePointEntity CompanyPlanUniquePoint1 { get; set; }

        public virtual CompanyPlanUniquePointEntity CompanyPlanUniquePoint2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnterLeavePointEventEntity> enter_leave_point_event { get; set; }
    }
}
