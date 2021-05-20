﻿using System;
using System.Linq;
using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class DetectorRepo : RepoBase<DetectorEntity>, IRepo<DetectorEntity>
    {
        public DetectorRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<DetectorEntity> SetInclude(IQueryable<DetectorEntity> entities)
        {
            return entities.Include(detector => detector.DetectorPrefab)
                           .Include(detector => detector.DetectorPrefab.DetectorSettingsPrefabs.Select(settings => settings.DataType))
                           .Include(detector => detector.DetectorPrefab.DetectorDataPrefabs.Select(data => data.DataType))
                           .Include(detector => detector.DetectorPrefab.DetectorFaultPrefabs)
                           .Include(detector => detector.DetectorSettingsValues)
                           .Include(detector => detector.DetectorDatas)
                           .Include(detector => detector.DetectorFaults);
        }
    }
}