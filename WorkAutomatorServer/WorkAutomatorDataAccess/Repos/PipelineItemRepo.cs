using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class PipelineItemRepo : RepoBase<PipelineItemEntity>, IRepo<PipelineItemEntity>
    {
        public PipelineItemRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<PipelineItemEntity> SetInclude(IQueryable<PipelineItemEntity> entities)
        {
            return entities.Include(item => item.Manufactory)
                           .Include(item => item.PipelineItemPrefab.PipelineItemSettingsPrefabs.Select(settings => settings.DataType))
                           .Include(item => item.PipelineItemSettingsValues)
                           .Include(item => item.InputPipelineItemConnections)
                           .Include(item => item.OutputPipelineItemConnections)
                           .Include(item => item.PipelineItemStorageConnections.Select(connection => connection.StorageCell));
        }
    }
}
