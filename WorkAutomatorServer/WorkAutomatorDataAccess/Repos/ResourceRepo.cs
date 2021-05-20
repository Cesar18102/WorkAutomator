using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class ResourceRepo : RepoBase<ResourceEntity>, IRepo<ResourceEntity>
    {
        public ResourceRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<ResourceEntity> SetInclude(IQueryable<ResourceEntity> entities)
        {
            return entities.Include(resource => resource.Unit)
                           .Include(resource => resource.Company);
        }
    }
}
