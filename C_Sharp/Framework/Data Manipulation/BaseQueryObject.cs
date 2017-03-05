using System;
using System.Collections.Generic;

namespace Framework.Data_Manipulation
{
    public interface IBaseQueryObject
    {
        Type SearchInputType { get; }
        object SearchInputObject { get; set; }
    }

    public interface IBaseQueryObject<TEntity> : IBaseQueryObject
    {
        IList<TEntity> Execute();
    }

    public interface IBaseQueryObject<TEntity, TSearchInput> : IBaseQueryObject<TEntity>
    {
        TSearchInput SearchInput { get; set; }
        IList<TEntity> PerformSearchOperation(TSearchInput searchInput);
    }

    public abstract class BaseQueryObject<TEntity, TSearchInput> :
        IBaseQueryObject<TEntity, TSearchInput>
    {
        public TSearchInput SearchInput { get; set; }

        public object SearchInputObject
        {
            get { return SearchInput; }
            set { SearchInput = (TSearchInput) value; }
        }

        public Type SearchInputType {
            get { return typeof(TSearchInput); }
        }

        public abstract IList<TEntity> PerformSearchOperation(TSearchInput searchInput);

        public IList<TEntity> Execute()
        {
            IList<TEntity> ultimateResult = PerformSearchOperation(SearchInput);

            return ultimateResult;
        }
    }
}
