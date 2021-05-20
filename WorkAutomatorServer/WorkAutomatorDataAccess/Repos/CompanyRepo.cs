using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class CompanyRepo : RepoBase<CompanyEntity>, IRepo<CompanyEntity>
    {
        public CompanyRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<CompanyEntity> SetInclude(IQueryable<CompanyEntity> entities)
        {
            return entities.Include(company => company.Owner.Roles)
                           .Include(company => company.Members.SelectMany(member => member.Roles))
                           .Include(company => company.CompanyPlanUniquePoints)
                           .Include(company => company.CompanyPlanUniquePoints.SelectMany(p => p.EnterLeavePoints))
                           .Include(company => company.Manufactories.Select(m => m.ManufactoryPlanPoints))
                           .Include(company => company.Manufactories.Select(m => m.CheckPoints))
                           .Include(company => company.Pipelines)
                           .Include(company => company.PipelineItemPrefabs)
                           .Include(company => company.StorageCellPrefabs)
                           .Include(company => company.DetectorPrefabs)
                           .Include(company => company.Resources.Select(resource => resource.Unit))
                           .Include(company => company.Units);
        }
    }
}
