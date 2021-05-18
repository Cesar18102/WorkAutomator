namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PipelineItemSettingsValueEntity : EntityBase
    {
        
        public int pipeline_item_id { get; set; }

        public int pipeline_item_settings_prefab_id { get; set; }

        [Column(TypeName = "text")]
        public string option_data_value_base64 { get; set; }

        public virtual PipelineItemEntity pipeline_item { get; set; }

        public virtual PipelineItemSettingsPrefabEntity pipeline_item_settings_prefab { get; set; }
    }
}
