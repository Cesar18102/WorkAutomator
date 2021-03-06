namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("check_point_event")]
    public partial class CheckPointEventEntity : IdEntity
    {
        
        public int check_point_id { get; set; }

        public int account_id { get; set; }

        public DateTime timespan { get; set; }

        public bool is_direct { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public virtual AccountEntity account { get; set; }

        public virtual CheckPointEntity check_point { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return check_point.IsOwnedByCompany(companyId);
        }
    }
}
