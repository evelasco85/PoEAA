package com;

import com.Interfaces.IUnitOfWork;
import com.Interfaces.UnitOfWorkAction;
import com.datamanipulation.BaseMapperInterfaces.IBaseMapper;
import com.datamanipulation.BaseMapperInterfaces.InvocationDelegates;
import com.domain.DomainObjectInterfaces.IDomainObject;

import java.rmi.NoSuchObjectException;
import java.util.HashMap;
import java.util.List;
import java.util.UUID;

/**
 * Created by aiko on 4/1/17.
 */

public class UnitOfWork implements IUnitOfWork {
    HashMap<UUID, IDomainObject> _insertionObjects = new HashMap<UUID, IDomainObject>();
    HashMap<UUID, IDomainObject> _updatingObjects = new HashMap<UUID, IDomainObject>();
    HashMap<UUID, IDomainObject> _deletionObjects = new HashMap<UUID, IDomainObject>();

    boolean ContainsKey(HashMap<UUID, IDomainObject> domainDictionary, IDomainObject domainObject)
    {
        return domainDictionary.containsKey(domainObject.GetSystemId());
    }

    void AddEntity(HashMap<UUID, IDomainObject> domainDictionary, IDomainObject domainObject)
    {
        domainDictionary.put(domainObject.GetSystemId(), domainObject);
    }

    void RemoveEntity(HashMap<UUID, IDomainObject> domainDictionary, IDomainObject domainObject)
    {
        if (ContainsKey(domainDictionary, domainObject))
            domainDictionary.remove(domainObject.GetSystemId());
    }

    <TEntity extends IDomainObject> void ValidateEntityPrerequisites(TEntity entity)
            throws NoSuchObjectException
    {
        if (entity == null)
            throw new NoSuchObjectException("'entity' parameter is required");

        if (entity.GetMapper() == null)
            throw new NoSuchObjectException("A 'mapper' implementation is required for an entity to be observed");
    }

    public <TEntity extends IDomainObject> TEntity RegisterNew(TEntity entity)
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

            AddEntity(_insertionObjects, entity);
        }
        catch (NoSuchObjectException exception)
        {
            throw exception;
        }

        return entity;
    }

    public <TEntity extends IDomainObject> TEntity RegisterDirty(TEntity entity)
            throws NoSuchObjectException
    {
        try {
            ValidateEntityPrerequisites(entity);

            if (ContainsKey(_deletionObjects, entity))
                throw new UnsupportedOperationException(
                        "'entity' already registered for deletion | [Operation Register: Dirty]");

            if (ContainsKey(_insertionObjects, entity) || ContainsKey(_updatingObjects, entity))
                return entity;

            AddEntity(_updatingObjects, entity);
        }
        catch (NoSuchObjectException exception)
        {
            throw exception;
        }

        return entity;
    }

    public <TEntity extends IDomainObject> TEntity RegisterRemoved(TEntity entity)
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

            AddEntity(_deletionObjects, entity);


        }
        catch (NoSuchObjectException exception)
        {
            throw exception;
        }

        return entity;
    }

    public void Commit(UoWInvocationDelegates delegates)
    {

    }

    public void ClearUnitOfWork()
    {
        _insertionObjects.clear();
        _updatingObjects.clear();
        _deletionObjects.clear();
    }

    public boolean PendingCommits(){
        return (!_insertionObjects.isEmpty()) || (!_updatingObjects.isEmpty()) || (!_deletionObjects.isEmpty());
    }

    void ApplyOperation(
            UnitOfWorkAction action, List<IDomainObject> affectedEntities,
            InvocationDelegates delegates
    )
    {
        for (int index = 0; index < affectedEntities.size(); index++)
        {
            IDomainObject entity = affectedEntities.get(index);

            if(entity == null)
                continue;

            IBaseMapper mapper = entity.GetMapper();

            switch (action)
            {
                case Insert:
                    mapper.Insert(entity, delegates);
                    break;
                case Update:
                    mapper.Update(entity, delegates);
                    break;
                case Delete:
                    mapper.Delete(entity, delegates);
                    break;
            }
        }
    }
}
