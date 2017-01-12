using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IQueryObject<TEntity> _queryObject = DataSynchronizationManager.GetInstance().GetQueryObject<TEntity>();
        private IList<TEntity> _loadedEntities = DataSynchronizationManager.GetInstance().GetLoadedEntities<TEntity>();

        IList<TEntity> LocalLookup(IBaseCriteria<TEntity> criteria)
        {
            throw new NotImplementedException();
        }

        void ConsolidateResults(IList<TEntity> newResult)
        {
            if ((newResult == null) || (!newResult.Any()))
                return;
         
            ((List<TEntity>)_loadedEntities).AddRange(newResult);
        }

        public IList<TEntity> Matching(IBaseCriteria<TEntity> criteria)
        {
            IList<TEntity> localResult = LocalLookup(criteria);

            if ((localResult == null) || (!localResult.Any()))
            {
                localResult = _queryObject.Execute(criteria);

                ConsolidateResults(localResult);
            }

            return localResult;
        }
    }
}
