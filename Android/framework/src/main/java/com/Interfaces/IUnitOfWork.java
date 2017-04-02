package com.Interfaces;

import com.domain.DomainObjectInterfaces.IDomainObject;

import java.rmi.NoSuchObjectException;

/**
 * Created by aiko on 4/1/17.
 */

public interface IUnitOfWork {
    <TEntity extends IDomainObject> TEntity RegisterNew(TEntity entity)
            throws NoSuchObjectException;

    <TEntity extends IDomainObject> TEntity RegisterDirty(TEntity entity)
            throws NoSuchObjectException;

    <TEntity extends IDomainObject> TEntity RegisterRemoved(TEntity entity)
            throws NoSuchObjectException;

    void Commit(UoWInvocationDelegates delegates);
    void ClearUnitOfWork();
    boolean PendingCommits();
}
