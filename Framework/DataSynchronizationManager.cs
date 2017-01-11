using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{

    public interface IDataSynchronizationManager
    {
        IBaseRepository<TEntity> GetRepository<TEntity>()
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

        IList<object> _serviceContainerList = new List<object>();

        private DataSynchronizationManager()
        {
        }

        public static IDataSynchronizationManager GetInstance()
        {
            return s_instance;
        }

        public void RegisterEntity<TEntity>(IBaseRepository<TEntity> repository, IBaseMapper<TEntity> mapper, IBaseQueryObject<TEntity> queryObject)
            where TEntity : IDomainObject
        {
            IEntityServiceContainer<TEntity> serviceContainer = new EntityServiceContainer<TEntity>
            {
                LoadedEntities = new List<TEntity>(),
                Mapper = mapper,
                QueryObject = queryObject,
                Repository = repository
            };

            _serviceContainerList.Add(serviceContainer);
        }

        IEntityServiceContainer<TEntity> GetServiceContainer<TEntity>()
            where TEntity : IDomainObject
        {
            return null;
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>()
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
