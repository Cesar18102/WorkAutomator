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
            return entities.Include(company => company.Owner)
                           .Include(company => company.Members)
                           .Include(company => company.CompanyPlanUniquePoints)
                           .Include(company => company.Manufactories)
                           .Include(company => company.Pipelines)
                           .Include(company => company.PipelineItemPrefabs)
                           .Include(company => company.StorageCellPrefabs)
                           .Include(company => company.DetectorPrefabs)
                           .Include(company => company.Resources)
                           .Include(company => company.Units)
                           .Include(company => company.Roles)
                           .Include(company => company.Tasks);
        }
    }
}
