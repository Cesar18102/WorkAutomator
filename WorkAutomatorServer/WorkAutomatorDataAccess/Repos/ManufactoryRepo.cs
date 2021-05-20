using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class ManufactoryRepo : RepoBase<ManufactoryEntity>, IRepo<ManufactoryEntity>
    {
        public ManufactoryRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<ManufactoryEntity> SetInclude(IQueryable<ManufactoryEntity> entities)
        {
            return entities.Include(manufactory => manufactory.PipelineItems.Select(pi => pi.PipelineItemPrefab))
                           .Include(manufactory => manufactory.StorageCells.Select(sc => sc.StorageCellPrefab))
                           .Include(manufactory => manufactory.CheckPoints)
                           .Include(manufactory => manufactory.CheckPoints.Select(p => p.ManufactoryFrom))
                           .Include(manufactory => manufactory.CheckPoints.Select(p => p.ManufactoryTo))
                           .Include(manufactory => manufactory.ManufactoryPlanPoints.Select(p => p.CompanyPlanUniquePoint))
                           .Include(manufactory => manufactory.Company);
        }
    }
}
