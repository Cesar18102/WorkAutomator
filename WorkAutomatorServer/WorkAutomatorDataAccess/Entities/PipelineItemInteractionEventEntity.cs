namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pipeline_item_interaction_event")]
    public partial class PipelineItemInteractionEventEntity : IdEntity
    {
        
        public int pipeline_item_id { get; set; }

        public int account_id { get; set; }

        public DateTime timespan { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public virtual AccountEntity account { get; set; }

        public virtual PipelineItemEntity pipeline_item { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return pipeline_item.IsOwnedByCompany(companyId);
        }
    }
}
