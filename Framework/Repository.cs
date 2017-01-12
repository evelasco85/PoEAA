using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IRepository<TEntity>
        where TEntity : IDomainObject
    {
        IList<TEntity> Matching(IBaseCriteria<TEntity> criteria);
    }

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : IDomainObject
    {
        private readonly IBaseQueryObject<TEntity> _queryObject = DataSynchronizationManager.GetInstance().GetQueryObject<TEntity>();
        private readonly IBaseMapper<TEntity> _mapper = DataSynchronizationManager.GetInstance().GetMapper<TEntity>();
        private IList<TEntity> _loadedEntities = DataSynchronizationManager.GetInstance().GetLoadedEntities<TEntity>();

        public IList<TEntity> Matching(IBaseCriteria<TEntity> criteria)
        {
            throw new NotImplementedException();
        }
    }
}
