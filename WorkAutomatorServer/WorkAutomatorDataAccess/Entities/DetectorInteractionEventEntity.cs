namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("detector_interaction_event")]
    public partial class DetectorInteractionEventEntity : IdEntity
    {
        
        public int detector_id { get; set; }

        public int account_id { get; set; }

        public DateTime timespan { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public virtual AccountEntity account { get; set; }

        public virtual DetectorEntity detector { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return detector.IsOwnedByCompany(companyId);
        }
    }
}
