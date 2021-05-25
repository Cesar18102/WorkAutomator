using System;
using System.Data.Entity;
using System.Threading.Tasks;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess
{
    public class UnitOfWork : IDisposable
    {
        public enum ContextType
        {
            REAL,
            TEST
        }

        private DbContext Context { get; set; }
        private bool IsDisposed { get; set; }

        public UnitOfWork() : this(typeof(WorkAutomatorDBContext)) { }

        public UnitOfWork(Type contextType)
        {
            if (!typeof(DbContext).IsAssignableFrom(contextType))
                throw new InvalidOperationException("contextType must be derived from DbContext");

            Context = contextType.GetConstructor(new Type[] { })
                                 .Invoke(new object[] { }) as DbContext;
        }

        public IRepo<TEntity> GetRepo<TEntity>() where TEntity : EntityBase
        {
            return new RepoBase<TEntity>(Context);
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                    Context.Dispose();

                this.IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
