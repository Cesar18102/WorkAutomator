using System;
using System.Linq;
using System.Reflection;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.Exceptions;
using WorkAutomatorDataAccess.RepoInterfaces;
using System.Linq.Expressions;

namespace WorkAutomatorDataAccess.Repos
{
    internal class RepoBase<TContext, TEntity> : IRepo<TEntity> 
        where TContext : DbContext, new()
        where TEntity : EntityBase
    {
        protected virtual IQueryable<TEntity> SetInclude(IQueryable<TEntity> entities) => entities;

        public virtual async Task<TEntity> Get(int id)
        {
            return await this.FirstOrDefault(e => e.id == id);
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            using (TContext db = new TContext())
            {
                return await SetInclude(
                    db.Set<TEntity>().Where(predicate)
                ).FirstOrDefaultAsync();
            }
        }

        public virtual async Task<IList<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            using (TContext db = new TContext())
            {
                return await SetInclude(
                    db.Set<TEntity>().Where(predicate)
                ).ToListAsync();
            }
        }

        public virtual async Task<IList<TEntity>> Get()
        {
            using (TContext db = new TContext())
            {
                return await SetInclude(
                    db.Set<TEntity>()
                ).ToListAsync();
            }
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            using(TContext db = new TContext())
            {
                TEntity added = db.Set<TEntity>().Add(entity);
                await db.SaveChangesAsync();
                return await this.Get(added.id);
            }
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            using(TContext db = new TContext())
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
            using (TContext db = new TContext())
            {
                TEntity found = db.Set<TEntity>().FirstOrDefault(entity => entity.id == id);

                if (found == null)
                    throw new EntityNotFoundException($"{typeof(TEntity).Name} with Id = {id}");

                db.Set<TEntity>().Remove(found);
                await db.SaveChangesAsync();
            }
        }

        public async Task Clear()
        {
            using (TContext db = new TContext())
            {
                db.Set<TEntity>().RemoveRange(db.Set<TEntity>());
                await db.SaveChangesAsync();
            }
        }

        protected TEntity ProtectedExecute(Func<TEntity, TEntity> executor, TEntity entity)
        {
            try { return executor(entity); }
            catch (DatabaseActionValidationException ex)
            {
                InvalidDataException<TEntity> invalidDataException = new InvalidDataException<TEntity>();

                foreach (ValidationResult validationResult in ex.Errors)
                {
                    foreach (string invalidFieldName in validationResult.MemberNames)
                    {
                        InvalidFieldInfo<TEntity> invalidFieldInfo = new InvalidFieldInfo<TEntity>(invalidFieldName, validationResult.ErrorMessage);
                        invalidDataException.InvalidFieldInfos.Add(invalidFieldInfo);
                    }
                }

                throw invalidDataException;
            }
        }

        private void CopyProperties(TEntity source, TEntity target)
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
    }
}
