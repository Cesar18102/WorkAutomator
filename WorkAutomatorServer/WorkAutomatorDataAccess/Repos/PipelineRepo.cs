using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class PipelineRepo : RepoBase<PipelineEntity>, IRepo<PipelineEntity>
    {
        public PipelineRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<PipelineEntity> SetInclude(IQueryable<PipelineEntity> entities)
        {
            return entities.Include(pipeline => pipeline.Company)
                           .Include(pipeline => pipeline.PipelineItems)
                           .Include(pipeline => pipeline.PipelineItems.Select(item => item.Manufactory))
                           .Include(pipeline => pipeline.PipelineItems.Select(item => item.PipelineItemPrefab))
                           .Include(pipeline => pipeline.PipelineItems.SelectMany(item => item.InputPipelineItemConnections))
                           .Include(pipeline => pipeline.PipelineItems.SelectMany(item => item.OutputPipelineItemConnections))
                           .Include(pipeline => pipeline.PipelineItems.SelectMany(item => item.PipelineItemStorageConnections.Select(connection => connection.StorageCell)));
        }
    }
}
