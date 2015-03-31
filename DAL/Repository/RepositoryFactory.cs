using System;
using System.Collections.Generic;
using System.Data.Entity;
using Filewatcher.MDL;

namespace Filewatcher.DAL
{
    public class RepositoryFactories
    {
        private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;

        private static IDictionary<Type, Func<DbContext, object>> GetFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
            {
                {typeof(IObservableRepository<Watch>), dbContext => new ObservableRepository<Watch>(dbContext)},
                {typeof(IObservableRepository<History>), dbContext => new ObservableRepository<History>(dbContext)}
            };
        }

        public RepositoryFactories()
        {
            _repositoryFactories = GetFactories();
        }

        public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories)
        {
            _repositoryFactories = factories;
        }

        public Func<DbContext, object> GetRepositoryFactory<T>()
        {
            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class, IEntity
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        protected virtual Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class, IEntity
        {
            return dbContext => new Repository<T>(dbContext);
        }
    }
}