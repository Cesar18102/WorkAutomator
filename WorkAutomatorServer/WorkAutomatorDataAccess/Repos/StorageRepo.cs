using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class StorageRepo : RepoBase<StorageCellEntity>, IRepo<StorageCellEntity>
    {
        public StorageRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<StorageCellEntity> SetInclude(IQueryable<StorageCellEntity> entities)
        {
            return entities.Include(cell => cell.StorageCellPrefab)
                           .Include(cell => cell.ResourcesAtStorageCell.Select(res => res.Resource));
        }
    }
}
