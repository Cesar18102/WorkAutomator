namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PipelineItemInteractionEventEntity : EntityBase
    {
        
        public int pipeline_item_id { get; set; }

        public int account_id { get; set; }

        public DateTime timespan { get; set; }

        [Column(TypeName = "text")]
        public string log { get; set; }

        public virtual AccountEntity account { get; set; }

        public virtual PipelineItemEntity pipeline_item { get; set; }
    }
}
