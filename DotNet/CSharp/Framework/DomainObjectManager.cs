using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using Framework.Data_Manipulation;

namespace Framework
{
    public interface IMapperRegistry
    {
        IBaseMapper<TEntity> GetMapper<TEntity>();
    }

    public interface IRepositoryRegistry
    {
        IRepository<TEntity> GetRepository<TEntity>();
    }

    public interface IQueryObjectRegistry
    {
        IBaseQueryObject<TEntity> GetQueryBySearchCriteria<TEntity>(string searchInputTypename);
    }

    public interface IDomainObjectManager : IMapperRegistry, IRepositoryRegistry, IQueryObjectRegistry
    {
        void RegisterEntity<TEntity>(IBaseMapper<TEntity> mapper, IList<IBaseQueryObject<TEntity>> queryList);

        IDictionary<string, PropertyInfo> GetProperties<TEntity>();
    }

    public class DomainObjectManager : IDomainObjectManager
    {
        IDictionary<string, object> _serviceContainerDictionary = new Dictionary<string, object>();

        public DomainObjectManager()
        {
        }

        string GetServiceContainerKey<TEntity>()
        {
            return typeof(TEntity).FullName;
        }

        public void RegisterEntity<TEntity>(IBaseMapper<TEntity> mapper, IList<IBaseQueryObject<TEntity>> queryList)
        {
            IEntityServiceContainer<TEntity> serviceContainer = new EntityServiceContainer<TEntity>
            {
                Mapper = mapper,
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
        {
            IDictionary<string, IBaseQueryObject<TEntity>>  queryDictionary = new Dictionary<string, IBaseQueryObject<TEntity>>();

            if ((queryList == null) || (!queryList.Any()))
                return queryDictionary;

            for (int index = 0; index < queryList.Count; index++)
            {
                IBaseQueryObject<TEntity> query = queryList[index];

                if(query == null)
                    continue;

                queryDictionary.Add(query.CriteriaInputType.FullName, query);
            }

            return queryDictionary;
        }

        bool ServiceContainerExists(string key)
        {
            return _serviceContainerDictionary.ContainsKey(key);
        }

        IEntityServiceContainer<TEntity> GetServiceContainer<TEntity>()
        {
            string key = GetServiceContainerKey<TEntity>();

            if (!ServiceContainerExists(key))
                throw new InstanceNotFoundException(string.Format("Service container with key '{0}' not found.", key));

            IEntityServiceContainer<TEntity> serviceContainer = (IEntityServiceContainer<TEntity>)_serviceContainerDictionary[key];

            return serviceContainer;
        }

        public IRepository<TEntity> GetRepository<TEntity>()
        {
            return GetServiceContainer<TEntity>().Repository;
        }

        public IBaseMapper<TEntity> GetMapper<TEntity>()
        {
            return GetServiceContainer<TEntity>().Mapper;
        }

        public IBaseQueryObject<TEntity> GetQueryBySearchCriteria<TEntity>(string searchInputTypename)
        {
            IEntityServiceContainer<TEntity> serviceContainer = GetServiceContainer<TEntity>();

            return serviceContainer.QueryDictionary[searchInputTypename];
        }

        public IDictionary<string, PropertyInfo> GetProperties<TEntity>()
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
