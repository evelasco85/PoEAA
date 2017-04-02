package com;

import com.Interfaces.IUnitOfWork;
import com.Interfaces.UnitOfWorkAction;
import com.Interfaces.UoWInvocationDelegates;
import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.DataManipulation.BaseMapperInterfaces.InvocationDelegates;
import com.Domain.DomainObjectInterfaces.IDomainObject;

import java.rmi.NoSuchObjectException;
import java.util.Collection;
import java.util.HashMap;
import java.util.UUID;

/**
 * Created by aiko on 4/1/17.
 */

public class UnitOfWork implements IUnitOfWork {
    HashMap<UUID, IDomainObject> _insertionObjects = new HashMap<UUID, IDomainObject>();
    HashMap<UUID, IDomainObject> _updatingObjects = new HashMap<UUID, IDomainObject>();
    HashMap<UUID, IDomainObject> _deletionObjects = new HashMap<UUID, IDomainObject>();
    HashMap<UUID, InvocationDelegates> _invocationDelegates = new HashMap<UUID, InvocationDelegates>();

    boolean ContainsKey(HashMap<UUID, IDomainObject> domainDictionary, IDomainObject domainObject)
    {
        return domainDictionary.containsKey(domainObject.GetSystemId());
    }

    void AddEntity(HashMap<UUID, IDomainObject> domainDictionary, IDomainObject domainObject, InvocationDelegates invocationDelegates)
    {
        domainDictionary.put(domainObject.GetSystemId(), domainObject);
        _invocationDelegates.put(domainObject.GetSystemId(), invocationDelegates);
    }

    void RemoveEntity(HashMap<UUID, IDomainObject> domainDictionary, IDomainObject domainObject)
    {
        if (ContainsKey(domainDictionary, domainObject)) {
            domainDictionary.remove(domainObject.GetSystemId());
            _invocationDelegates.remove(domainObject.GetSystemId());
        }
    }

    <TEntity extends IDomainObject> void ValidateEntityPrerequisites(TEntity entity)
            throws NoSuchObjectException
    {
        if (entity == null)
            throw new NoSuchObjectException("'entity' parameter is required");

        if (entity.GetMapper() == null)
            throw new NoSuchObjectException("A 'mapper' implementation is required for an entity to be observed");
    }

    public <TEntity extends IDomainObject> TEntity RegisterNew(TEntity entity, InvocationDelegates invocationDelegates)
            throws NoSuchObjectException
    {
        try {
            ValidateEntityPrerequisites(entity);

            if (ContainsKey(_updatingObjects, entity))
                throw new UnsupportedOperationException("'entity' already registered for update | [Operation Register: New]");

            if (ContainsKey(_deletionObjects, entity))
                throw new UnsupportedOperationException("'entity' already registered for deletion | [Operation Register: New]");

            if (ContainsKey(_insertionObjects, entity))
                return entity;

            AddEntity(_insertionObjects, entity, invocationDelegates);
        }
        catch (NoSuchObjectException exception)
        {
            throw exception;
        }

        return entity;
    }

    public <TEntity extends IDomainObject> TEntity RegisterDirty(TEntity entity, InvocationDelegates invocationDelegates)
            throws NoSuchObjectException
    {
        try {
            ValidateEntityPrerequisites(entity);

            if (ContainsKey(_deletionObjects, entity))
                throw new UnsupportedOperationException(
                        "'entity' already registered for deletion | [Operation Register: Dirty]");

            if (ContainsKey(_insertionObjects, entity) || ContainsKey(_updatingObjects, entity))
                return entity;

            AddEntity(_updatingObjects, entity, invocationDelegates);
        }
        catch (NoSuchObjectException exception)
        {
            throw exception;
        }

        return entity;
    }

    public <TEntity extends IDomainObject> TEntity RegisterRemoved(TEntity entity, InvocationDelegates invocationDelegates)
            throws NoSuchObjectException
    {
        try {
            ValidateEntityPrerequisites(entity);

            if (ContainsKey(_insertionObjects, entity) || ContainsKey(_updatingObjects, entity))
            {
                RemoveEntity(_insertionObjects, entity);
                RemoveEntity(_updatingObjects, entity);

                return entity;
            }

            if (ContainsKey(_deletionObjects, entity))
                return entity;

            AddEntity(_deletionObjects, entity, invocationDelegates);
        }
        catch (NoSuchObjectException exception)
        {
            throw exception;
        }

        return entity;
    }

    public void Commit(UoWInvocationDelegates delegates)
    {
        ApplyOperation(UnitOfWorkAction.Insert, _insertionObjects.values(), delegates);
        ApplyOperation(UnitOfWorkAction.Update, _updatingObjects.values(), delegates);
        ApplyOperation(UnitOfWorkAction.Delete, _deletionObjects.values(), delegates);
        ClearUnitOfWork();
    }

    public void ClearUnitOfWork()
    {
        _insertionObjects.clear();
        _updatingObjects.clear();
        _deletionObjects.clear();
        _invocationDelegates.clear();
    }

    public boolean PendingCommits(){
        return (!_insertionObjects.isEmpty()) || (!_updatingObjects.isEmpty()) || (!_deletionObjects.isEmpty());
    }

    void ApplyOperation(
            UnitOfWorkAction action, Collection<IDomainObject> affectedEntities,
            UoWInvocationDelegates uoWInvocationDelegates
    )
    {
        for (IDomainObject entity : affectedEntities) {

            if (entity == null)
                continue;

            IBaseMapper mapper = entity.GetMapper();
            InvocationDelegates invocation = _invocationDelegates.get(entity.GetSystemId());
            boolean success = false;

            if (invocation != null)
                invocation.SetResults(null);

            switch (action) {
                case Insert:
                    success = mapper.Insert(entity, invocation);
                    break;
                case Update:
                    success = mapper.Update(entity, invocation);
                    break;
                case Delete:
                    success = mapper.Delete(entity, invocation);
                    break;
            }

            if (invocation != null)
                uoWInvocationDelegates.SetResults(invocation.GetResults());

            if (success)
                uoWInvocationDelegates.SuccessfulUoWInvocationDelegate(entity, action);
            else
                uoWInvocationDelegates.FailedUoWInvocationDelegate(entity, action);
        }
    }
}
