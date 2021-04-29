using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess
{
    internal class WorkAutomatorDBContext : DbContext
    {
        public WorkAutomatorDBContext() : base("WorkAutomatorDBContext")
        {

        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
