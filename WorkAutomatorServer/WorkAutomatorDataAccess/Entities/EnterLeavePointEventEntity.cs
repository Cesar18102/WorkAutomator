namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("enter_leave_point_event")]
    public partial class EnterLeavePointEventEntity : IdEntity
    {
        
        public int enter_leave_point_id { get; set; }

        public int account_id { get; set; }

        public DateTime timespan { get; set; }

        public bool is_enter { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public virtual AccountEntity account { get; set; }

        public virtual EnterLeavePointEntity enter_leave_point { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return enter_leave_point.IsOwnedByCompany(companyId);
        }
    }
}
