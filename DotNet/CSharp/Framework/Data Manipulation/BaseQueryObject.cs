using System;

namespace Framework.Data_Manipulation
{
    //Used for inferred declaration in generics(See Repository.Matching<...>(...)
    public interface ICriteriaTag<TEntity, TResult> { }

    public interface IBaseQueryObject
    {
        Type CriteriaInputType { get; }
        object CriteriaInputObject { get; set; }
    }

    public interface IBaseQueryObject<TEntity> : IBaseQueryObject
    {
        IBaseMapper<TEntity> GetMapper(IDomainObjectManager manager);
        object Execute(IDomainObjectManager manager);
    }

    public interface IBaseQueryObject<TEntity, TResult, TCriteriaInput> : IBaseQueryObject<TEntity>
        where TCriteriaInput : ICriteriaTag<TEntity, TResult>
    {
        TCriteriaInput CriteriaInput { get; set; }
        TResult PerformSearchOperation(IBaseMapper mapper, TCriteriaInput criteriaInput);
        TResult ConcreteExecute(IDomainObjectManager manager);
    }

    public abstract class BaseQueryObject<TEntity, TResult, TCriteriaInput> :
        IBaseQueryObject<TEntity, TResult, TCriteriaInput>
        where TCriteriaInput : ICriteriaTag<TEntity, TResult>
    {
        public TCriteriaInput CriteriaInput { get; set; }

        public object CriteriaInputObject
        {
            get { return CriteriaInput; }
            set { CriteriaInput = (TCriteriaInput) value; }
        }

        public Type CriteriaInputType {
            get { return typeof(TCriteriaInput); }
        }

        public abstract TResult PerformSearchOperation(IBaseMapper mapper, TCriteriaInput criteriaInput);

        public abstract IBaseMapper<TEntity> GetMapper(IDomainObjectManager manager);

        public TResult ConcreteExecute(IDomainObjectManager manager)
        {
            return PerformSearchOperation(GetMapper(manager), CriteriaInput);
        }

        public object Execute(IDomainObjectManager manager)
        {
            return ConcreteExecute(manager);
        }
    }
}
