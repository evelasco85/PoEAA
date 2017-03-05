using Framework.Domain;
using System.Collections;

namespace Framework.Data_Manipulation
{
    public delegate void SuccessfulInvocationDelegate(IDomainObject domainObject, Hashtable results);
    public delegate void FailedInvocationDelegate(IDomainObject domainObject, Hashtable results);

    public interface IBaseMapper_Instantiator<TEntity>
     where TEntity : IDomainObject
    {
    }

    public interface IBaseMapper
    {
        string GetEntityTypeName();
        TOut GetResultValue<TOut>(Hashtable resultsTable, string key);
        bool Update(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        bool Insert(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        bool Delete(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
    }

    public abstract class BaseMapper : IBaseMapper
    {
        public abstract string GetEntityTypeName();

        public static TOut GetHashValue<TOut>(Hashtable resultsTable, string key)
        {
            return ((resultsTable != null) && (resultsTable[key] != null)) ? (TOut)resultsTable[key] : default(TOut);
        }

        public TOut GetResultValue<TOut>(Hashtable resultsTable, string key)
        {
            return GetHashValue<TOut>(resultsTable, key);
        }

        public abstract bool Update(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        public abstract bool Insert(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        public abstract bool Delete(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
    }

    public interface IBaseMapper<TEntity> : IBaseMapper, IBaseMapper_Instantiator<TEntity>
        where TEntity : IDomainObject
    {
        bool Update(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        bool Insert(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        bool Delete(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
    }

    public abstract class BaseMapper<TEntity> : BaseMapper, IBaseMapper<TEntity>
        where TEntity : IDomainObject
    {
        public override string GetEntityTypeName()
        {
            return typeof(TEntity).Name;
        }

        public abstract bool Update(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        public abstract bool Insert(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        public abstract bool Delete(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);

        public override bool Update(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity)entity;

            SetSafeSuccessfulInvocator(ref successfulInvocation);
            SetSafeFailureInvocator(ref failedInvocation);

            return Update(ref instance, successfulInvocation, failedInvocation);
        }

        public override bool Insert(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity)entity;

            SetSafeSuccessfulInvocator(ref successfulInvocation);
            SetSafeFailureInvocator(ref failedInvocation);

            return Insert(ref instance, successfulInvocation, failedInvocation);
        }

        public override bool Delete(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity)entity;

            SetSafeSuccessfulInvocator(ref successfulInvocation);
            SetSafeFailureInvocator(ref failedInvocation);

            return Delete(ref instance, successfulInvocation, failedInvocation);
        }

        void SetSafeSuccessfulInvocator(ref SuccessfulInvocationDelegate successfulInvocation)
        {
            if (successfulInvocation == null)
                successfulInvocation = (domainObject, results) => { };
        }

        void SetSafeFailureInvocator(ref FailedInvocationDelegate failedInvocation)
        {
            if (failedInvocation == null)
                failedInvocation = (domainObject, results) => { };
        }
    }
}
