using System.Collections.Generic;
using System.Management.Instrumentation;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{

    public interface IDataSynchronizationManager
    {
        void RegisterEntity<TEntity>(IBaseMapper<TEntity> mapper, IBaseQueryObject<TEntity> queryObject)
            where TEntity : IDomainObject;
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : IDomainObject;
        IBaseQueryObject<TEntity> GetQueryObject<TEntity>()
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
            return typeof(TEntity).Name;
        }

        public void RegisterEntity<TEntity>(IBaseMapper<TEntity> mapper, IBaseQueryObject<TEntity> queryObject)
            where TEntity : IDomainObject
        {
            IEntityServiceContainer<TEntity> serviceContainer = new EntityServiceContainer<TEntity>
            {
                LoadedEntities = new List<TEntity>(),
                Repository = new Repository<TEntity>(),
                Mapper = mapper,
                QueryObject = queryObject
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
            IEntityServiceContainer<TEntity> serviceContainer = (IEntityServiceContainer<TEntity>)_serviceContainerDictionary[GetServiceContainerKey<TEntity>()];

            if (serviceContainer == null)
            {
                string key = GetServiceContainerKey<TEntity>(); 
                
                throw new InstanceNotFoundException(string.Format("Service container with key '{0}' not found.", key));
            }

            return serviceContainer;
        }

        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : IDomainObject
        {
            return GetServiceContainer<TEntity>().Repository;
        }

        public IBaseQueryObject<TEntity> GetQueryObject<TEntity>()
            where TEntity : IDomainObject
        {
            return GetServiceContainer<TEntity>().QueryObject;
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
