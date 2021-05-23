using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using NUnit.Framework;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;
using WorkAutomatorDataAccess.Exceptions;
using WorkAutomatorDataAccess.Aspects;

namespace DataAccessTests
{
    public abstract class RepoTestsBase<TEntity> where TEntity : EntityBase
    {
        protected static IRepo<TEntity> Repo = RepoDependencyHolder.Dependencies.ResolveKeyed<IRepo<TEntity>>(
            RepoDependencyHolder.ContextType.TEST
        );

        protected static List<TEntity> Inserted = new List<TEntity>();

        protected abstract Dictionary<TEntity, bool> GetDataForInsertTest();

        [Test]
        [Order(0)]
        public async Task ClearTest()
        {
            await Repo.Clear();
        }

        [Test]
        [Order(1)]
        public async Task GetFailTest()
        {
            Assert.IsNull(await Repo.Get(1));
        }

        [Test]
        [Order(2)]
        public async Task TryInsertTest()
        {
            Dictionary<TEntity, bool> dataToInsert = GetDataForInsertTest();

            foreach (KeyValuePair<TEntity, bool> entityToInsert in dataToInsert)
            {
                try
                {
                    TEntity inserted = await Repo.Create(entityToInsert.Key);

                    if (entityToInsert.Value)
                        Inserted.Add(inserted);
                    else
                        Assert.Fail("Succeed to insert, but should be not");
                }
                catch(DatabaseActionValidationException)
                {
                    if (entityToInsert.Value)
                        Assert.Fail("Failed to insert, but should be successful");
                }
            }
        }

        [Test]
        [Order(3)]
        public async Task CheckInsertedTest()
        {
            foreach (TEntity entity in Inserted)
                Assert.IsNotNull(await Repo.Get(entity.id));
        }

        [Test]
        [Order(4)]
        public async Task CleanupTest()
        {
            foreach (TEntity entity in Inserted)
                await Repo.Delete(entity.id);
        }
    }
}
