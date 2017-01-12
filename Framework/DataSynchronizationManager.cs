using System.Collections.Generic;
using System.Management.Instrumentation;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IDataSynchronizationManager
    {
        void RegisterEntity<TEntity>(IBaseMapper<TEntity> mapper)
            where TEntity : IDomainObject;
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : IDomainObject;
        IBaseMapper<TEntity> GetMapper<TEntity>()
            where TEntity : IDomainObject;
        IList<TEntity> GetLoadedEntities<TEntity>()
            where TEntity : IDomainObject;
    }

    public class DataSynchronizationManager : IDataSynchronizationManager
    {
        static IDataSynchronizationManager s_instance = new DataSynchronizationManager();

        IDictionary<string, object> _serviceContainerDictionary = new Dictionary<string, object>();

        private DataSynchronizationManager()
        {
        }

        public static IDataSynchronizationManager GetInstance()
        {
            return s_instance;
        }

        string GetServiceContainerKey<TEntity>()
            where TEntity : IDomainObject
        {
            return typeof(TEntity).FullName;
        }

        public void RegisterEntity<TEntity>(IBaseMapper<TEntity> mapper)
            where TEntity : IDomainObject
        {
            IEntityServiceContainer<TEntity> serviceContainer = new EntityServiceContainer<TEntity>
            {
                Mapper = mapper,
                LoadedEntities = new List<TEntity>(),
                Repository = new Repository<TEntity>(this),
            };

            string key = GetServiceContainerKey<TEntity>();

            if (ServiceContainerExists(key))
                _serviceContainerDictionary.Remove(key);

            _serviceContainerDictionary.Add(key, serviceContainer);
        }

        bool ServiceContainerExists<TEntity>()
            where TEntity : IDomainObject
        {
            return _serviceContainerDictionary.ContainsKey(GetServiceContainerKey<TEntity>());
        }

        bool ServiceContainerExists(string key)
        {
            return _serviceContainerDictionary.ContainsKey(key);
        }

        IEntityServiceContainer<TEntity> GetServiceContainer<TEntity>()
            where TEntity : IDomainObject
        {
            string key = GetServiceContainerKey<TEntity>();

            if (!ServiceContainerExists(key))
                throw new InstanceNotFoundException(string.Format("Service container with key '{0}' not found.", key));

            IEntityServiceContainer<TEntity> serviceContainer = (IEntityServiceContainer<TEntity>)_serviceContainerDictionary[key];

            return serviceContainer;
        }

        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : IDomainObject
        {
            return GetServiceContainer<TEntity>().Repository;
        }

        public IBaseMapper<TEntity> GetMapper<TEntity>()
           where TEntity : IDomainObject
        {
            return GetServiceContainer<TEntity>().Mapper;
        }

        public IList<TEntity> GetLoadedEntities<TEntity>()
           where TEntity : IDomainObject
        {
            return GetServiceContainer<TEntity>().LoadedEntities;
        }
    }
}
