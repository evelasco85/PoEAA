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
        private readonly IDataSynchronizationManager _manager;

        public Repository(IDataSynchronizationManager manager)
        {
            _manager = manager;
        }

        IList<TEntity> LocalLookup(IBaseCriteria<TEntity> criteria)
        {
            throw new NotImplementedException();
        }

        void ConsolidateResults(IList<TEntity> newResult)
        {
            if ((newResult == null) || (!newResult.Any()))
                return;
         
            ((List<TEntity>)_manager.GetLoadedEntities<TEntity>()).AddRange(newResult);
        }

        public IList<TEntity> Matching(IBaseCriteria<TEntity> criteria)
        {
            IList<TEntity> localResult = LocalLookup(criteria);

            if ((localResult == null) || (!localResult.Any()))
            {
                localResult = _manager.GetQueryObject<TEntity>().Execute(criteria);

                ConsolidateResults(localResult);
            }

            return localResult;
        }
    }
}
