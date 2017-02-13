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
        TEntity RegisterNew<TEntity>(TEntity entity)
            where TEntity : IDomainObject;
        TEntity RegisterDirty<TEntity>(TEntity entity)
            where TEntity : IDomainObject;
        TEntity RegisterRemoved<TEntity>(TEntity entity)
            where TEntity : IDomainObject;
        void Commit(SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation);
        void ClearUnitOfWork();
        bool PendingCommits();
    }

    public class UnitOfWork : IUnitOfWork
    {
        delegate bool OperationDelegate(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);

        IDictionary<Guid, IDomainObject> _insertionObjects = new Dictionary<Guid, IDomainObject>();
        IDictionary<Guid, IDomainObject> _updatingObjects = new Dictionary<Guid, IDomainObject>();
        IDictionary<Guid, IDomainObject> _deletionObjects = new Dictionary<Guid, IDomainObject>();

        bool ContainsKey(IDictionary<Guid, IDomainObject> domainDictionary, IDomainObject domainObject)
        {
            return domainDictionary.ContainsKey(domainObject.SystemId);
        }

        void AddEntity(IDictionary<Guid, IDomainObject> domainDictionary, IDomainObject domainObject)
        {
            domainDictionary.Add(domainObject.SystemId, domainObject);
        }

        void RemoveEntity(IDictionary<Guid, IDomainObject> domainDictionary, IDomainObject domainObject)
        {
            if (ContainsKey(domainDictionary, domainObject))
                domainDictionary.Remove(domainObject.SystemId);
        }

        public TEntity RegisterNew<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            ValidateEntityPrerequisites(entity);

            if (ContainsKey(_updatingObjects, entity))
                throw new InvalidOperationException("'entity' already registered for update | [Operation Register: New]");

            if (ContainsKey(_deletionObjects, entity))
                throw new InvalidOperationException("'entity' already registered for deletion | [Operation Register: New]");

            if (ContainsKey(_insertionObjects, entity))
                return entity;

            AddEntity(_insertionObjects, entity);

            return entity;
        }

        public TEntity RegisterDirty<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            ValidateEntityPrerequisites(entity);

            if (ContainsKey(_deletionObjects, entity))
                throw new InvalidOperationException(
                    "'entity' already registered for deletion | [Operation Register: Dirty]");

            if (ContainsKey(_insertionObjects, entity) || ContainsKey(_updatingObjects, entity))
                return entity;

            AddEntity(_updatingObjects, entity);

            return entity;
        }

        public TEntity RegisterRemoved<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            ValidateEntityPrerequisites(entity);

            if (ContainsKey(_insertionObjects, entity) || ContainsKey(_updatingObjects, entity))
            {
                RemoveEntity(_insertionObjects, entity);
                RemoveEntity(_updatingObjects, entity);

                return entity;
            }

            if (ContainsKey(_deletionObjects, entity))
                return entity;

            AddEntity(_deletionObjects, entity);

            return entity;

        }

        void ValidateEntityPrerequisites<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            if (entity == null)
                throw new ArgumentNullException("'entity' parameter is required");

            if (entity.Mapper == null)
                throw new NullReferenceException("A 'mapper' implementation is required for an entity to be observed");
        }

        public void Commit(SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation)
        {
            ApplyOperation(UnitOfWorkAction.Insert, _insertionObjects.Values.ToList(), successfulInvocation, failedInvocation);
            ApplyOperation(UnitOfWorkAction.Update, _updatingObjects.Values.ToList(), successfulInvocation, failedInvocation);
            ApplyOperation(UnitOfWorkAction.Delete, _deletionObjects.Values.ToList(), successfulInvocation, failedInvocation);
            ClearUnitOfWork();
        }

        public void ClearUnitOfWork()
        {
            _insertionObjects.Clear();
            _updatingObjects.Clear();
            _deletionObjects.Clear();
        }

        public bool PendingCommits()
        {
            return (_insertionObjects.Any()) || (_updatingObjects.Any()) || (_deletionObjects.Any());
        }

        void ApplyOperation(
            UnitOfWorkAction action, IList<IDomainObject> affectedEntities,
            SuccessfulUoWInvocationDelegate successfulInvocation, FailedUoWInvocationDelegate failedInvocation
            )
        {
            for (int index = 0; index < affectedEntities.Count; index++)
            {
                IDomainObject entity = affectedEntities[index];

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
