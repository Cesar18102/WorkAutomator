using System;
using System.Reflection;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Linq.Expressions;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.Exceptions;
using WorkAutomatorDataAccess.Aspects;

namespace WorkAutomatorDataAccess
{
    internal class RepoBase<TEntity> : IRepo<TEntity> where TEntity : EntityBase
    {
        private DbContext Context { get; set; }
        
        public RepoBase(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TEntity> Get(int id)
        {
            PropertyInfo keyProperty = typeof(TEntity).GetProperties().FirstOrDefault(
                property => property.GetCustomAttribute<KeyAttribute>() != null
            );

            ParameterExpression parameter = Expression.Parameter(typeof(TEntity));
            Expression body = Expression.Equal(
                Expression.Property(parameter, keyProperty), 
                Expression.Constant(id)
            );

            Expression<Func<TEntity, bool>> keySelector = 
                Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return await this.FirstOrDefault(keySelector);
        }

        public virtual async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual async Task<IList<TEntity>> Get()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        [AddRepoInfoToDatabaseActionValidationExceptionAspect]
        public virtual async Task<TEntity> Create(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            return entity;
        }

        [AddRepoInfoToDatabaseActionValidationExceptionAspect]
        public virtual async Task Delete(int id)
        {
            TEntity found = await Get(id);

            if (found == null)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} with Id = {id}");

            Context.Set<TEntity>().Remove(await Get(id));
        }

        public virtual async Task Delete(int[] ids)
        {
            foreach (int id in ids)
                await Delete(id);
        }

        [AddRepoInfoToDatabaseActionValidationExceptionAspect]
        public virtual void Clear()
        {
            Context.Set<TEntity>().RemoveRange(Context.Set<TEntity>());
        }
    }
}
