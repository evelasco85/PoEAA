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
        IList<TEntity> Matching<TSearchInput>(TSearchInput criteria);
    }

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : IDomainObject
    {
        private readonly IDataSynchronizationManager _manager;

        public Repository(IDataSynchronizationManager manager)
        {
            _manager = manager;
        }
        
        void ApplyDomainObjectSettings(ref IList<TEntity> newResult, IBaseQueryObject query)
        {
            if ((newResult == null) || (!newResult.Any()))
                return;

            IBaseMapper<TEntity> mapper = _manager.GetMapper<TEntity>();

            ((List<TEntity>)newResult).ForEach(entity =>
            {
                ((ISystemManipulation)entity).SetQueryObject(query);
                ((ISystemManipulation)entity).MarkAsClean();
            });
        }

        IList<TEntity> Matching(IBaseQueryObject<TEntity> query)
        {
            IList<TEntity> results = query.Execute();

            ApplyDomainObjectSettings(ref results, query);

            return results;
        }

        public IList<TEntity> Matching<TSearchInput>(TSearchInput criteria)
        {
            IBaseQueryObject<TEntity> query = GetQueryBySearchCriteria<TSearchInput>();

            query.SearchInputObject = criteria;

            return Matching(query);
        }

        IBaseQueryObject<TEntity> GetQueryBySearchCriteria<TSearchInput>()
        {
            return _manager.GetQueryBySearchCriteria<TEntity, TSearchInput>();
        }
    }
}
