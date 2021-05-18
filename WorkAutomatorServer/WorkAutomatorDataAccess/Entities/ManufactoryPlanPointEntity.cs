namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ManufactoryPlanPointEntity : EntityBase
    {
        
        public int manufactory_id { get; set; }

        public int company_plan_unique_point_id { get; set; }

        public virtual CompanyPlanUniquePointEntity company_plan_unique_point { get; set; }

        public virtual ManufactoryEntity manufactory { get; set; }
    }
}
