using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public enum UnitOfWorkAction
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }

    public delegate void SuccessfulUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action, object additionalInfo);
    public delegate void FailedUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action, Exception exception, object additionalInfo);

    public interface IUnitOfWork
    {
        IUnitOfWorkEntityWrapper<TEntity> ObserveEntityForChanges<TEntity>(TEntity entity)
            where TEntity : IDomainObject;
        void UnobserveEntity<TEntity>(IUnitOfWorkEntityWrapper<TEntity> entityWrapper)
            where TEntity : IDomainObject;
        void Commit(SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation);
    }

    public class UnitOfWork : IUnitOfWork
    {
        delegate bool OperationDelegate(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);

        IDictionary<Guid, IUnitOfWorkEntityWrapper> _wrappedEntities = new Dictionary<Guid, IUnitOfWorkEntityWrapper>();

        public IUnitOfWorkEntityWrapper<TEntity> ObserveEntityForChanges<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            if(entity == null)
                throw  new ArgumentNullException("'entity' parameter is required");

            if (entity.Mapper == null)
                throw new NullReferenceException("A 'mapper' implementation is required for an entity to be observed");

            if (_wrappedEntities.ContainsKey(entity.SystemId))
            {
                return ((IUnitOfWorkEntityWrapper<TEntity>) _wrappedEntities[entity.SystemId]);
            }

            IUnitOfWorkEntityWrapper<TEntity> wrapper = new UnitOfWorkEntityWrapper<TEntity>(entity);

            _wrappedEntities.Add(wrapper.SystemId, wrapper);

            return wrapper;
        }

        public void UnobserveEntity<TEntity>(IUnitOfWorkEntityWrapper<TEntity> entityWrapper)
           where TEntity : IDomainObject
        {
            if (entityWrapper == null)
                throw new ArgumentNullException("'entityWrapper' parameter is required");

            if (!_wrappedEntities.ContainsKey(entityWrapper.SystemId))
                return;

            _wrappedEntities.Remove(entityWrapper.SystemId);
        }

        public void Commit(SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation)
        {
            ApplyOperation(UnitOfWorkAction.Insert, _wrappedEntities, successfulInvocation, failedInvocation);
            ApplyOperation(UnitOfWorkAction.Update, _wrappedEntities, successfulInvocation, failedInvocation);
            ApplyOperation(UnitOfWorkAction.Delete, _wrappedEntities, successfulInvocation, failedInvocation);
        }

        void ApplyOperation(
            UnitOfWorkAction action, IDictionary<Guid, IUnitOfWorkEntityWrapper> wrappedEntities,
            SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation
            )
        {
            List<IUnitOfWorkEntityWrapper> affectedEntities = wrappedEntities
                .Values
                .Where(objects => objects.GetExpectedAction() == action)
                .OrderBy(entity => entity.GetTicksUpdated())
                .ToList();

            for (int index = 0; index < affectedEntities.Count; index++)
            {
                IDomainObject entity = affectedEntities[index].EntityObject;

                if(entity == null)
                    continue;

                IBaseMapper mapper = entity.Mapper;
                OperationDelegate operation = GetOperation(action, mapper);

                bool success = operation(ref entity,
                    (domainObject, additionalInfo) =>
                    {
                        if (successfulInvocation != null)
                            successfulInvocation(domainObject, action, additionalInfo);
                    },
                    (domainObject, exception, additionalInfo) =>
                    {
                        if (failedInvocation != null)
                            failedInvocation(domainObject, action, exception, additionalInfo);
                    });

                //if(success)
                //    ((ISystemManipulation)entity).MarkAsClean();
            }
        }

        OperationDelegate GetOperation(UnitOfWorkAction action, IBaseMapper mapper)
        {
            OperationDelegate operation = null;

            switch (action)
            {
                case UnitOfWorkAction.Insert:
                    operation = new OperationDelegate(mapper.Insert);
                    break;
                case UnitOfWorkAction.Update:
                    operation = new OperationDelegate(mapper.Update);
                    break;
                case UnitOfWorkAction.Delete:
                    operation = new OperationDelegate(mapper.Delete);
                    break;
            }

            return operation;
        }
    }
}
