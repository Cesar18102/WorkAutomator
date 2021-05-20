using System;
using System.Reflection;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Linq.Expressions;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.Exceptions;
using WorkAutomatorDataAccess.RepoInterfaces;
using WorkAutomatorDataAccess.Aspects;

namespace WorkAutomatorDataAccess.Repos
{
    internal class RepoBase<TEntity> : IRepo<TEntity> where TEntity : EntityBase
    {
        public Type ContextType { get; private set; }
        public RepoBase(Type contextType)
        {
            if (!typeof(DbContext).IsAssignableFrom(contextType))
                throw new InvalidOperationException("contextType must be derived from DbContext");

            ContextType = contextType;
        }

        protected virtual IQueryable<TEntity> SetInclude(IQueryable<TEntity> entities) => entities;

        public virtual async Task<TEntity> Get(int id)
        {
            return await this.FirstOrDefault(e => e.id == id);
        }

        public virtual async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            using (DbContext db = CreateContext())
            {
                return await SetInclude(
                    db.Set<TEntity>().Where(predicate)
                ).FirstOrDefaultAsync();
            }
        }

        public virtual async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            using (DbContext db = CreateContext())
            {
                return await SetInclude(
                    db.Set<TEntity>().Where(predicate)
                ).ToListAsync();
            }
        }

        public virtual async Task<IList<TEntity>> Get()
        {
            using (DbContext db = CreateContext())
            {
                return await SetInclude(
                    db.Set<TEntity>()
                ).ToListAsync();
            }
        }

        [ProtectedExecuteAspect]
        public virtual async Task<TEntity> Create(TEntity entity)
        {
            using(DbContext db = CreateContext())
            {
                TEntity added = db.Set<TEntity>().Add(entity);
                await db.SaveChangesAsync();
                return await this.Get(added.id);
            }
        }

        [ProtectedExecuteAspect]
        public virtual async Task<TEntity> Update(TEntity entity)
        {
            using(DbContext db = CreateContext())
            {
                TEntity updatingEntity = db.Set<TEntity>().FirstOrDefault(e => e.id == entity.id);

                if (updatingEntity == null)
                    throw new EntityNotFoundException($"{typeof(TEntity).Name} with Id = {entity.id}");

                CopyProperties(entity, updatingEntity);
                await db.SaveChangesAsync();

                return await this.Get(updatingEntity.id);
            }
        }

        public virtual async Task Delete(int id)
        {
            using (DbContext db = CreateContext())
            {
                TEntity found = db.Set<TEntity>().FirstOrDefault(entity => entity.id == id);

                if (found == null)
                    throw new EntityNotFoundException($"{typeof(TEntity).Name} with Id = {id}");

                db.Set<TEntity>().Remove(found);
                await db.SaveChangesAsync();
            }
        }

        public virtual async Task Clear()
        {
            using (DbContext db = CreateContext())
            {
                db.Set<TEntity>().RemoveRange(db.Set<TEntity>());
                await db.SaveChangesAsync();
            }
        }

        protected void CopyProperties(TEntity source, TEntity target)
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            foreach(PropertyInfo property in properties)
            {
                if (property.GetCustomAttribute<DatabaseGeneratedAttribute>() != null)
                    continue;

                object value = property.GetValue(source);
                property.SetValue(target, value);
            }
        }

        protected DbContext CreateContext()
        {
            return ContextType.GetConstructor(new Type[] { }).Invoke(new object[] { }) as DbContext;
        }
    }
}
