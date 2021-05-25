using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class DbPermissionRepo : RepoBase<DbPermissionEntity>, IRepo<DbPermissionEntity>
    {
        public DbPermissionRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<DbPermissionEntity> SetInclude(IQueryable<DbPermissionEntity> entities)
        {
            return entities.Include(permission => permission.DbPermissionType);
        }
    }
}
