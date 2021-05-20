using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class RoleRepo : RepoBase<RoleEntity>, IRepo<RoleEntity>
    {
        public RoleRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<RoleEntity> SetInclude(IQueryable<RoleEntity> entities)
        {
            return entities.Include(role => role.Company)
                           .Include(role => role.DbPermissions.Select(p => p.DbPermissionType))
                           .Include(role => role.StorageCellPermissions)
                           .Include(role => role.PipelineItemPermissions)
                           .Include(role => role.ManufactoryPermissions)
                           .Include(role => role.DetectorPermissions);
        }
    }
}
