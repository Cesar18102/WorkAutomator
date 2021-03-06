using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess
{
    public interface IRepo<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> Get(int id);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<IList<TEntity>> Get();
        Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Create(TEntity item);

        Task Delete(int[] ids);
        Task Delete(int id);

        void Clear();
    }
}
