using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess.RepoInterfaces
{
    public interface IRepo<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> Get(int id);
        Task<IList<TEntity>> Get();
        Task<IList<TEntity>> Get(Predicate<TEntity> predicate);
        Task<TEntity> Create(TEntity item);
        Task<TEntity> Update(TEntity item);
        Task Delete(int id);
    }
}
