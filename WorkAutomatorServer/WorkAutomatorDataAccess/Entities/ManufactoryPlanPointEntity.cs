namespace WorkAutomatorDataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("manufactory_plan_point")]
    public partial class ManufactoryPlanPointEntity : IdEntity
    {
        
        public int manufactory_id { get; set; }

        public int company_plan_unique_point_id { get; set; }

        public virtual CompanyPlanUniquePointEntity CompanyPlanUniquePoint { get; set; }

        public virtual ManufactoryEntity manufactory { get; set; }

        public override bool IsOwnedByCompany(int companyId)
        {
            return CompanyPlanUniquePoint.IsOwnedByCompany(companyId);
        }
    }
}
