using Framework.Data_Manipulation;
using Framework.Domain;
using System;

namespace Framework
{
    public interface IRepository<TEntity>
    {
        TResult Matching<TResult>(ICriteriaTag<TEntity, TResult> criteria);
    }

    public class Repository<TEntity> : IRepository<TEntity>
    {
        private readonly IDataSynchronizationManager _manager;

        public Repository(IDataSynchronizationManager manager)
        {
            _manager = manager;
        }

        public TResult Matching<TResult>(ICriteriaTag<TEntity, TResult> criteria)
        {
            Type type = criteria.GetType();
            string searchInputTypename = type.FullName;
            IBaseQueryObject<TEntity> query = _manager.GetQueryBySearchCriteria<TEntity>(searchInputTypename);

            query.CriteriaInputObject = criteria;

            return (TResult)query.Execute();
        }
    }
}
