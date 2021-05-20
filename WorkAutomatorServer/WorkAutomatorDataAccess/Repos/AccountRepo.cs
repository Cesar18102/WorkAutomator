using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

using WorkAutomatorDataAccess.Entities;
using WorkAutomatorDataAccess.RepoInterfaces;

namespace WorkAutomatorDataAccess.Repos
{
    internal class AccountRepo : RepoBase<AccountEntity>, IRepo<AccountEntity>
    {
        public AccountRepo(Type contextType) : base(contextType) { }

        protected override IQueryable<AccountEntity> SetInclude(IQueryable<AccountEntity> entities)
        {
            return entities.Include(account => account.Subs)
                           .Include(account => account.Bosses)
                           .Include(account => account.OwnedCompany)
                           .Include(account => account.Company)
                           .Include(account => account.AssignedTasks)
                           .Include(account => account.AssignedTasks.Select(t => t.Assignee))
                           .Include(account => account.AssignedTasks.Select(t => t.Reviewer))
                           .Include(account => account.TasksToReview)
                           .Include(account => account.TasksToReview.Select(t => t.Assignee))
                           .Include(account => account.TasksToReview.Select(t => t.Reviewer))
                           .Include(account => account.Roles);
        }

        public override async Task Delete(int id)
        {
            using (DbContext db = CreateContext())
            {
                AccountEntity account = await db.Set<AccountEntity>().FirstOrDefaultAsync(acc => acc.id == id);

                account.Subs.Clear();
                account.Bosses.Clear();

                db.Set<AccountEntity>().Remove(account);
                await db.SaveChangesAsync();
            }
        }

        public override async Task Clear()
        {
            using(DbContext db = CreateContext())
            {
                await db.Set<AccountEntity>().ForEachAsync(account =>
                {
                    account.Subs.Clear();
                    account.Bosses.Clear();
                });

                db.Set<AccountEntity>().RemoveRange(db.Set<AccountEntity>());
                await db.SaveChangesAsync();
            }
        }
    }
}
