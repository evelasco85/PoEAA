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
        void ObserveEntityForChanges<TEntity>(TEntity entity)
            where TEntity : IDomainObject;
        void Commit(SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation);
        int GetUncommitedCount();
    }

    public class UnitOfWork : IUnitOfWork
    {
        delegate bool OperationDelegate(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);

        IDictionary<Guid, IDomainObject> _observedDomainObjects = new Dictionary<Guid, IDomainObject>();

        public void ObserveEntityForChanges<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            if(entity == null)
                throw  new ArgumentNullException("'entity' parameter is required");

            if (entity.Mapper == null)
                throw new NullReferenceException("A 'mapper' implementation is required for an entity to be observed");

            if(_observedDomainObjects.ContainsKey(entity.SystemId))
                return;

            

            _observedDomainObjects.Add(entity.SystemId, entity);
        }

        public void Commit(SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation)
        {
            ApplyOperation(DomainObjectState.Manually_Created, _observedDomainObjects, successfulInvocation, failedInvocation);
            ApplyOperation(DomainObjectState.Dirty, _observedDomainObjects, successfulInvocation, failedInvocation);
            ApplyOperation(DomainObjectState.For_DataSource_Deletion, _observedDomainObjects, successfulInvocation, failedInvocation);
        }

        public int GetUncommitedCount()
        {
            return _observedDomainObjects
                .Values
                .Where(objects => objects.GetCurrentState() != DomainObjectState.Clean)
                .Count();
        }

        void ApplyOperation(
            DomainObjectState affectedState, IDictionary<Guid, IDomainObject> domainObjects,
            SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation
            )
        {
            List<IDomainObject> affectedEntities = domainObjects
                .Values
                .Where(objects => objects.GetCurrentState() == affectedState)
                .OrderBy(objects => ((ISystemManipulation)objects).GetTicksUpdated())
                .ToList();

            for (int index = 0; index < affectedEntities.Count; index++)
            {
                IDomainObject entity = affectedEntities[index];

                if(entity == null)
                    continue;

                IBaseMapper mapper = entity.Mapper;
                Tuple<UnitOfWorkAction, OperationDelegate> operation = GetOperation(affectedState, mapper);

                bool success = operation.Item2(ref entity,
                    (domainObject, additionalInfo) =>
                    {
                        if (successfulInvocation != null)
                            successfulInvocation(domainObject, operation.Item1, additionalInfo);
                    },
                    (domainObject, exception, additionalInfo) =>
                    {
                        if (failedInvocation != null)
                            failedInvocation(domainObject, operation.Item1, exception, additionalInfo);
                    });

                if(success)
                    ((ISystemManipulation)entity).MarkAsClean();
            }
        }

        Tuple<UnitOfWorkAction, OperationDelegate> GetOperation(DomainObjectState state, IBaseMapper mapper)
        {
            OperationDelegate operation = null;
            UnitOfWorkAction action = UnitOfWorkAction.None;

            switch (state)
            {
                case DomainObjectState.Manually_Created:
                    action = UnitOfWorkAction.Insert;
                    operation = new OperationDelegate(mapper.Insert);
                    break;
                case DomainObjectState.Dirty:
                    action = UnitOfWorkAction.Update;
                    operation = new OperationDelegate(mapper.Update);
                    break;
                case DomainObjectState.For_DataSource_Deletion:
                    action = UnitOfWorkAction.Delete;
                    operation = new OperationDelegate(mapper.Delete);
                    break;
            }

            return new Tuple<UnitOfWorkAction, OperationDelegate>(action, operation);
        }
    }
}
