using System;
using Framework.Domain;

namespace Framework.Data_Manipulation
{
    public delegate void SuccessfulInvocationDelegate(IDomainObject domainObject);
    public delegate void FailedInvocationDelegate(IDomainObject domainObject, Exception exception);

    public interface IBaseMapper_Instantiator<TEntity>
     where TEntity : IDomainObject
    {
        TEntity CreateEntity();
    }

    public interface IBaseMapper
    {
        void Update(ref object entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        void Insert(ref object entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        void Delete(ref object entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
    }

    public interface IBaseMapper<TEntity> : IBaseMapper, IBaseMapper_Instantiator<TEntity>
        where TEntity: IDomainObject
    {
        void Update(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        void Insert(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        void Delete(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
    }

    public abstract class BaseMapper<TEntity> : IBaseMapper<TEntity>
        where TEntity : IDomainObject
    {
        public TEntity CreateEntity()
        {
            TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));

            entity.SystemId = Guid.NewGuid();
            entity.Mapper = this;

            return entity;
        }

        public abstract void Update(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        public abstract void Insert(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        public abstract void Delete(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);

        public void Update(ref object entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity) entity;

            Update(ref instance, successfulInvocation, failedInvocation);
        }

        public void Insert(ref object entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity)entity;

            Insert(ref instance, successfulInvocation, failedInvocation);
        }

        public void Delete(ref object entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity)entity;

            Delete(ref instance, successfulInvocation, failedInvocation);
        }
    }
}
