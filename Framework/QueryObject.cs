using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IQueryObject<TEntity>
        where TEntity : IDomainObject
    {
        IList<TEntity> Execute(IBaseCriteria<TEntity> criteria);
    }

    public class QueryObject<TEntity> : IQueryObject<TEntity>
        where TEntity : IDomainObject
    {
        private readonly IDataSynchronizationManager _manager;

        public QueryObject(IDataSynchronizationManager manager)
        {
            _manager = manager;
        }

        public IList<TEntity> Execute(IBaseCriteria<TEntity> criteria)
        {
            if(criteria == null)
                throw new ArgumentNullException("value for 'criteria' parameter is required");

            return criteria.Execute();
        }
    }
}
