namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pipeline_item_connection")]
    public partial class PipelineItemConnectionEntity : IdEntity
    {
        
        public int pipeline_item1_id { get; set; }

        public int pipeline_item2_id { get; set; }

        public bool is_direct { get; set; }

        public virtual PipelineItemEntity SourcePipelineItem { get; set; }

        public virtual PipelineItemEntity TargetPipelineItem { get; set; }
    }
}
