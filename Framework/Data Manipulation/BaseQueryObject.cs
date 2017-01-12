using System;
using System.Collections.Generic;
using Framework.Domain;

namespace Framework.Data_Manipulation
{
    public interface IBaseQueryObject<TEntity>
    {
        IList<TEntity> Execute();
    }

    public interface IBaseQueryObject<TEntity, TSearchInput, TSearchResult> : IBaseQueryObject<TEntity>
    {
        TSearchInput SearchInput { get; set; }
        TSearchResult PerformSearchOperation(TSearchInput searchInput);
        IList<TEntity> ConstructOutput(TSearchResult searchResult);
    }

    public abstract class BaseQueryObject<TEntity, TSearchInput, TSearchResult> :
        IBaseQueryObject<TEntity, TSearchInput, TSearchResult>
    {
        public TSearchInput SearchInput { get; set; }

        public abstract TSearchResult PerformSearchOperation(TSearchInput searchInput);

        public abstract IList<TEntity> ConstructOutput(TSearchResult searchResult);

        public IList<TEntity> Execute()
        {
            TSearchResult searchResult = PerformSearchOperation(SearchInput);
            IList<TEntity> ultimateResult = ConstructOutput(searchResult);

            return ultimateResult;
        }
    }
}
