using System.Collections.Generic;
using System.Linq;
using Framework.Domain;

namespace Framework.Data_Manipulation
{
    public interface IBaseRepository<TEntity>
        where TEntity : IDomainObject
    {
    }

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : IDomainObject
    {
        private readonly IBaseQueryObject<TEntity> _queryObject = DataSynchronizationManager.GetInstance().GetQueryObject<TEntity>();
        private readonly IBaseMapper<TEntity> _mapper = DataSynchronizationManager.GetInstance().GetMapper<TEntity>();
        private IList<TEntity> _loadedEntities = DataSynchronizationManager.GetInstance().GetLoadedEntities<TEntity>();

        public BaseRepository()
        {
        }

        public abstract IList<TEntity> Matching(IBaseCriteria<TEntity> criteria);

        public IList<TEntity> GetEntities()
        {
            return _loadedEntities;
        }
    }
}
