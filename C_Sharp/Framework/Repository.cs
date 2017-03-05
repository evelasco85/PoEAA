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

            IBaseMapper<TEntity> mapper = DataSynchronizationManager.GetInstance().GetMapper<TEntity>();

            ((List<TEntity>)newResult).ForEach(entity =>
            {
                ((ISystemManipulation)entity).SetQueryObject(query);
                ((ISystemManipulation)entity).SetMapper(mapper);
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
            IBaseQueryObject<TEntity> query = _manager.GetQueryBySearchCriteria<TEntity, TSearchInput>();

            query.SearchInputObject = criteria;

            return Matching(query);
        }
    }
}
