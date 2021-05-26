namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("check_point")]
    public partial class CheckPointEntity : IdEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CheckPointEntity()
        {
            check_point_event = new HashSet<CheckPointEventEntity>();
        }

        [Column("company_plan_unique_point1_id")]
        public int CompanyPlanUniquePoint1Id { get; set; }

        [Column("company_plan_unique_point2_id")]
        public int CompanyPlanUniquePoint2Id { get; set; }

        [Column("manufactory1_id")]
        public int Manufactory1Id { get; set; }

        [Column("manufactory2_id")]
        public int Manufactory2Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckPointEventEntity> check_point_event { get; set; }

        public virtual CompanyPlanUniquePointEntity CompanyPlanUniquePoint1 { get; set; }

        public virtual CompanyPlanUniquePointEntity CompanyPlanUniquePoint2 { get; set; }

        public virtual ManufactoryEntity Manufactory1 { get; set; }

        public virtual ManufactoryEntity Manufactory2 { get; set; }
    }
}
