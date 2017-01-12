using System;
using System.Collections.Generic;
using Framework.Domain;

namespace Framework.Data_Manipulation
{
    public interface IBaseCriteria<TEntity>
       where TEntity : IDomainObject
    {
        IList<TEntity> Execute();
    }

    public interface IBaseCriteria<TEntity, TSearchInput, TSearchResult> : IBaseCriteria<TEntity>
        where TEntity : IDomainObject
    {
        TSearchInput SearchInput { get; set; }
        TSearchResult PerformSearchOperation(TSearchInput searchInput);
        IList<TEntity> ConstructOutput(TSearchResult searchResult, IBaseMapper_Instantiator<TEntity> mapper);
        void ChainOperationResultToDownstreamCriteria(
            IBaseCriteria<TEntity, TSearchResult, IList<TEntity>> downstreamCriteria);
    }

    public abstract class BaseCriteria<TEntity, TSearchInput, TSearchResult> :
        IBaseCriteria<TEntity, TSearchInput, TSearchResult>
        where TEntity : IDomainObject
    {
        private IBaseCriteria<TEntity, TSearchResult, IList<TEntity>> _downstreamCriteria;

        public TSearchInput SearchInput { get; set; }

        public abstract TSearchResult PerformSearchOperation(TSearchInput searchInput);

        public virtual IList<TEntity> ConstructOutput(TSearchResult searchResult, IBaseMapper_Instantiator<TEntity> mapper)
        {
            //Implementation deferred to ultimate processor
            throw new NotImplementedException();
        }

        public void ChainOperationResultToDownstreamCriteria(
            IBaseCriteria<TEntity, TSearchResult, IList<TEntity>> downstreamCriteria)
        {
            _downstreamCriteria = downstreamCriteria;
        }

        public IList<TEntity> Execute()
        {
            TSearchResult searchResult = PerformSearchOperation(SearchInput);

            IList<TEntity> ultimateResult;

            if (_downstreamCriteria != null)
            {
                _downstreamCriteria.SearchInput = searchResult;

                ultimateResult = _downstreamCriteria.Execute();
            }
            else
            {
                IBaseMapper<TEntity> mapper = DataSynchronizationManager.GetInstance().GetMapper<TEntity>();

                ultimateResult = ConstructOutput(searchResult, mapper);
            }

            return ultimateResult;
        }
    }
}
