using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using NUnit.Framework;

using WorkAutomatorDataAccess;
using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.Exceptions;

namespace DataAccessTests
{
    public abstract class RepoTestsBase<TEntity> where TEntity : IdEntity
    {
        protected static List<TEntity> Inserted = new List<TEntity>();
        protected static UnitOfWork DB = new UnitOfWork();

        protected abstract Dictionary<TEntity, bool> GetDataForInsertTest();

        [OneTimeTearDown]
        public void End()
        {
            DB.Dispose();
            Inserted.Clear();
        }

        [Test]
        [Order(1)]
        public async Task GetFailTest()
        {
            Assert.IsNull(await DB.GetRepo<TEntity>().Get(-1));
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
                    TEntity inserted = await DB.GetRepo<TEntity>().Create(entityToInsert.Key);

                    if (entityToInsert.Value)
                    {
                        Inserted.Add(inserted);
                        await DB.Save();
                    }
                    else
                        Assert.Fail("Succeed to insert, but should be not");
                }
                catch (DatabaseActionValidationException)
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
                Assert.IsNotNull(await DB.GetRepo<TEntity>().Get(entity.id));
        }

        [Test]
        [Order(4)]
        public async Task CleanupTest()
        {
            foreach (TEntity entity in Inserted)
                await DB.GetRepo<TEntity>().Delete(entity.id);
        }
    }
}
