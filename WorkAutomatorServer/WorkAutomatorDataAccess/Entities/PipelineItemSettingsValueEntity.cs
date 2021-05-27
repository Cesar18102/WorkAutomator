namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pipeline_item_settings_value")]
    public partial class PipelineItemSettingsValueEntity : IdEntity
    {
        
        public int pipeline_item_id { get; set; }

        public int pipeline_item_settings_prefab_id { get; set; }

        [Column(TypeName = "text")]
        public string option_data_value_base64 { get; set; }

        public virtual PipelineItemEntity pipeline_item { get; set; }

        public virtual PipelineItemSettingsPrefabEntity PipelineItemSettingsPrefab { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return PipelineItemSettingsPrefab.IsOwnedByCompany(companyId);
        }
    }
}
