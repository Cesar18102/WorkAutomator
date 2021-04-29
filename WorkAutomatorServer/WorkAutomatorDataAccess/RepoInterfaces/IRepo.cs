using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess.RepoInterfaces
{
    public interface IRepo<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> Create(TEntity item);
        Task<TEntity> Update(TEntity item);
        Task Delete(int id);
        TEntity Get(int id);
        Task<IList<TEntity>> GetAll();
        Task<IList<TEntity>> Get(Predicate<TEntity> predicate);
    }
}
