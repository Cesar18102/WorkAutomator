using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos.PrefabRepos
{
    internal class DetectorPrefabRepo : RepoBase<DetectorPrefabEntity>, IRepo<DetectorPrefabEntity>
    {
        public DetectorPrefabRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<DetectorPrefabEntity> SetInclude(IQueryable<DetectorPrefabEntity> entities)
        {
            return entities.Include(prefab => prefab.DetectorSettingsPrefabs.Select(settings => settings.DataType))
                           .Include(prefab => prefab.DetectorDataPrefabs.Select(data => data.DataType))
                           .Include(prefab => prefab.DetectorFaultPrefabs);
        }
    }
}
