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
        void ApplySystemSettings(ref TEntity entity);
        void ApplyExternalSourceConfigurations(ref TEntity entity);
    }

    public interface IBaseMapper
    {
        bool Update(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        bool Insert(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        bool Delete(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
    }

    public interface IBaseMapper<TEntity> : IBaseMapper, IBaseMapper_Instantiator<TEntity>
        where TEntity: IDomainObject
    {
        bool Update(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        bool Insert(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        bool Delete(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
    }

    public abstract class BaseMapper<TEntity> : IBaseMapper<TEntity>
        where TEntity : IDomainObject
    {
        public TEntity CreateEntity()
        {
            TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));

            ApplySystemSettings(ref entity);

            return entity;
        }

        public void ApplySystemSettings(ref TEntity entity)
        {
            entity.Mapper = this;
            entity.SystemId = Guid.NewGuid();
        }

        public void ApplyExternalSourceConfigurations(ref TEntity entity)
        {
            ((ISystemManipulation)entity).MarkAsClean();
        }

        public abstract bool Update(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        public abstract bool Insert(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
        public abstract bool Delete(ref TEntity entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);

        public bool Update(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity) entity;

            return Update(ref instance, successfulInvocation, failedInvocation);
        }

        public bool Insert(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity)entity;

            return Insert(ref instance, successfulInvocation, failedInvocation);
        }

        public bool Delete(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            TEntity instance = (TEntity)entity;

            return Delete(ref instance, successfulInvocation, failedInvocation);
        }
    }
}
