using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using WorkAutomatorDataAccess.Entities;

namespace DataAccessTests
{
    [TestFixture]
    public class AccountRepoTests : RepoTestsBase<AccountEntity>
    {
        protected override Dictionary<AccountEntity, bool> GetDataForInsertTest()
        {
            Dictionary<AccountEntity, bool> data = new Dictionary<AccountEntity, bool>();

            data.Add(
                new AccountEntity()
                {
                    login = "testLogin",
                    password = "testPassword",
                    first_name = "testFirstName",
                    last_name = "testLastName"
                }, true
            );

            data.Add(
                new AccountEntity()
                {
                    login = "testLogin",
                    password = "testPassword",
                    first_name = "testFirstName",
                    last_name = "testLastName"
                }, false
            );

            return data;
        }

        [Test]
        [Order(5)]
        public async Task AddRemoveBossSubsRelationTest()
        {
            await Repo.Clear();

            AccountEntity sub = new AccountEntity()
            {
                login = "testSub",
                password = "testPassword",
                first_name = "testFirstName",
                last_name = "testLastName"
            };

            AccountEntity boss = new AccountEntity()
            {
                login = "testBoss",
                password = "testPassword",
                first_name = "testFirstName",
                last_name = "testLastName",
            };

            boss.Subs.Add(sub);

            AccountEntity addedBoss = await Repo.Create(boss);
            Assert.IsNotEmpty(addedBoss.Subs);
        }
    }
}