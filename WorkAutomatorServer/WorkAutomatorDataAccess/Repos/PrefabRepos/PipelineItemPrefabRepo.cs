using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos.PrefabRepos
{
    internal class PipelineItemPrefabRepo : RepoBase<PipelineItemPrefabEntity>, IRepo<PipelineItemPrefabEntity>
    {
        public PipelineItemPrefabRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<PipelineItemPrefabEntity> SetInclude(IQueryable<PipelineItemPrefabEntity> entities)
        {
            return entities.Include(prefab => prefab.PipelineItemSettingsPrefabs.Select(settings => settings.DataType));
        }
    }
}
