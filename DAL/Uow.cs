using System;
using Filewatcher.MDL;

namespace Filewatcher.DAL
{
    public class Uow : IUow, IDisposable
    {
        public Uow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        public IObservableRepository<Watch> Watches { get { return GetRepo<IObservableRepository<Watch>>(); } }
        public IObservableRepository<History> Histories { get { return GetRepo<IObservableRepository<History>>(); } }
        public void Commit()
        {
            DbContext.SaveChanges();
        }

        protected void CreateDbContext()
        {
            DbContext = new DatabaseContext();

            DbContext.Configuration.ProxyCreationEnabled = true;
            DbContext.Configuration.LazyLoadingEnabled = true;
            DbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        private IRepository<T> GetStandardRepo<T>() where T : class, IEntity
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }
        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        private DatabaseContext DbContext { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (DbContext != null)
            {
                DbContext.Dispose();
            }
        }
    }
}