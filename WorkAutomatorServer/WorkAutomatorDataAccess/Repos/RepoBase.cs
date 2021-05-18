using System;
using System.Linq;
using System.Reflection;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.Exceptions;

namespace WorkAutomatorDataAccess.Repos
{
    internal class RepoBase<TEntity> where TEntity : EntityBase
    {
        //protected TEntity ProtectedExecute(Func<TEntity, TEntity> executor, TEntity entity)
        //{
            //try { return executor(entity); }
            //catch (DatabaseActionValidationException ex)
            //{
            //    InvalidDataException<TModel> invalidDataException = new InvalidDataException<TModel>();
            //    foreach (ValidationResult validationResult in ex.Errors)
            //    {
            //        string wrongField = validationResult.MemberNames.First();

            //        IEnumerable<PropertyMap> propertyMaps = Mapper.ConfigurationProvider.FindTypeMapFor<TModel, TEntity>().PropertyMaps;
            //        PropertyMap propertyMap = propertyMaps.First(pmap => pmap.DestinationMember.Name == wrongField);

            //        string dtoWrongFieldName = propertyMap.SourceMember.Name;
            //        string messageUpdated = validationResult.ErrorMessage.Replace(wrongField, "'" + dtoWrongFieldName + "'");

            //        InvalidFieldInfo<TModel> invalidFieldInfo = new InvalidFieldInfo<TModel>(dtoWrongFieldName, messageUpdated);
            //        invalidDataException.InvalidFieldInfos.Add(invalidFieldInfo);
            //    }

            //    throw invalidDataException;
            //}
        //}

        protected virtual TEntity SingleInclude(TEntity entity) => entity;
        protected virtual IQueryable<TEntity> SetInclude(IQueryable<TEntity> entities) => entities;

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            using(WorkAutomatorDBContext db = new WorkAutomatorDBContext())
            {
                TEntity added = db.Set<TEntity>().Add(entity);
                await db.SaveChangesAsync();
                return SingleInclude(added);
            }
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            using(WorkAutomatorDBContext db = new WorkAutomatorDBContext())
            {
                TEntity updatingEntity = db.Set<TEntity>().FirstOrDefault(e => e.Id == entity.Id);

                if (updatingEntity == null)
                    throw new EntityNotFoundException($"{typeof(TEntity).Name} with Id = {entity.Id}");

                CopyProperties(entity, updatingEntity);
                await db.SaveChangesAsync();

                return SingleInclude(updatingEntity);
            }
        }

        public virtual async Task Delete(int id)
        {
            using (WorkAutomatorDBContext db = new WorkAutomatorDBContext())
            {
                TEntity found = db.Set<TEntity>().FirstOrDefault(entity => entity.Id == id);

                if (found == null)
                    throw new EntityNotFoundException($"{typeof(TEntity).Name} with Id = {id}");

                db.Set<TEntity>().Remove(found);
                await db.SaveChangesAsync();
            }
        }

        public virtual async Task<TEntity> Get(int id)
        {
            using (WorkAutomatorDBContext db = new WorkAutomatorDBContext())
            {
                TEntity found = db.Set<TEntity>().FirstOrDefault(entity => entity.Id == id);
                return found == null ? null : SingleInclude(found);
            }
        }

        public virtual async Task<IList<TEntity>> Get(Predicate<TEntity> predicate)
        {
            using (WorkAutomatorDBContext db = new WorkAutomatorDBContext())
            {
                List<TEntity> entities = db.Set<TEntity>().ToList();

                return await SetInclude(
                    db.Set<TEntity>().Where(
                        entity => predicate(entity)
                    )
                ).ToListAsync();
            }
        }

        public virtual async Task<IList<TEntity>> Get()
        {
            using (WorkAutomatorDBContext db = new WorkAutomatorDBContext())
            {
                List<TEntity> entities = db.Set<TEntity>().ToList();
                return await SetInclude(db.Set<TEntity>()).ToListAsync();
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
