using Framework.Domain;
using System;

namespace Framework.Data_Manipulation
{
    //Used for inferred declaration in generics(See Repository.Matching<...>(...)
    public interface ICriteriaTag<TResult> { }

    public interface IBaseQueryObject
    {
        Type CriteriaInputType { get; }
        object CriteriaInputObject { get; set; }
    }

    public interface IBaseQueryObject<TEntity> : IBaseQueryObject
        where TEntity : IDomainObject
    {
        IBaseMapper<TEntity> GetMapper();
        object Execute();
    }

    public interface IBaseQueryObject<TEntity, TResult, TCriteriaInput> : IBaseQueryObject<TEntity>
        where TEntity : IDomainObject
        where TCriteriaInput : ICriteriaTag<TResult>
    {
        TCriteriaInput CriteriaInput { get; set; }
        TResult PerformSearchOperation(IBaseMapper mapper, TCriteriaInput criteriaInput);
        TResult ConcreteExecute();
    }

    public abstract class BaseQueryObject<TEntity, TResult, TCriteriaInput> :
        IBaseQueryObject<TEntity, TResult, TCriteriaInput>
        where TEntity : IDomainObject
        where TCriteriaInput : ICriteriaTag<TResult>
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

        public abstract IBaseMapper<TEntity> GetMapper();

        public TResult ConcreteExecute()
        {
            return PerformSearchOperation(GetMapper(), CriteriaInput);
        }

        public object Execute()
        {
            return ConcreteExecute();
        }
    }
}
