using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IMapperRegistry
    {
        IBaseMapper<TEntity> GetMapper<TEntity>()
            where TEntity : IDomainObject;
    }

    public interface IRepositoryRegistry
    {
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : IDomainObject;
    }

    public interface IQueryObjectRegistry
    {
        IBaseQueryObject<TEntity> GetQueryBySearchCriteria<TEntity, TSearchInput>()
            where TEntity : IDomainObject;
    }

    public interface IIdentityMapRegistry
    {
        IIdentityMap<TEntity> GetIdentityMap<TEntity>()
            where TEntity : IDomainObject;
    }

    public interface IDataSynchronizationManager : IMapperRegistry, IRepositoryRegistry, IQueryObjectRegistry
    {
        void RegisterEntity<TEntity>(IBaseMapper<TEntity> mapper, IList<IBaseQueryObject<TEntity>> queryList)
            where TEntity : IDomainObject;

        IDictionary<string, PropertyInfo> GetProperties<TEntity>()
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

        public void RegisterEntity<TEntity>(IBaseMapper<TEntity> mapper, IList<IBaseQueryObject<TEntity>> queryList)
            where TEntity : IDomainObject
        {
            IEntityServiceContainer<TEntity> serviceContainer = new EntityServiceContainer<TEntity>
            {
                Mapper = mapper,
                IdentityMap = new IdentityMap<TEntity>(),
                Repository = new Repository<TEntity>(this),
                QueryDictionary = ConvertQueryListToDictionary(queryList),
                PrimitiveProperties = GetPrimitiveProperties<TEntity>()
            };

            string key = GetServiceContainerKey<TEntity>();

            if (ServiceContainerExists(key))
                _serviceContainerDictionary.Remove(key);

            _serviceContainerDictionary.Add(key, serviceContainer);
        }

        IDictionary<string, IBaseQueryObject<TEntity>> ConvertQueryListToDictionary<TEntity>(IList<IBaseQueryObject<TEntity>> queryList)
            where TEntity : IDomainObject
        {
            IDictionary<string, IBaseQueryObject<TEntity>>  queryDictionary = new Dictionary<string, IBaseQueryObject<TEntity>>();

            if ((queryList == null) || (!queryList.Any()))
                return queryDictionary;

            for (int index = 0; index < queryList.Count; index++)
            {
                IBaseQueryObject<TEntity> query = queryList[index];

                if(query == null)
                    continue;

                queryDictionary.Add(query.SearchInputType.FullName, query);
            }

            return queryDictionary;
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

        public IBaseQueryObject<TEntity> GetQueryBySearchCriteria<TEntity, TSearchInput>()
          where TEntity : IDomainObject
        {
            IEntityServiceContainer<TEntity> serviceContainer = GetServiceContainer<TEntity>();

            return serviceContainer.QueryDictionary[typeof(TSearchInput).FullName];
        }

        public IIdentityMap<TEntity> GetIdentityMap<TEntity>()
            where TEntity : IDomainObject
        {
            IEntityServiceContainer<TEntity> serviceContainer = GetServiceContainer<TEntity>();

            return serviceContainer.IdentityMap;
        }

        public IDictionary<string, PropertyInfo> GetProperties<TEntity>()
           where TEntity : IDomainObject
        {
            IEntityServiceContainer<TEntity> serviceContainer = GetServiceContainer<TEntity>();

            return serviceContainer.PrimitiveProperties;
        }

        IDictionary<string, PropertyInfo> GetPrimitiveProperties<TEntity>()
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            Type entityType = typeof(TEntity);

            return entityType
                .GetProperties(flags)
                .Where(property => (!property.PropertyType.IsClass) || (property.PropertyType == typeof(string)))
                .ToDictionary(property => property.Name, property => property);
        }
    }
}
