package com.Interfaces;

import com.DataManipulation.BaseMapperInterfaces.InvocationDelegates;
import com.Domain.Interfaces.IDomainObject;

import java.rmi.NoSuchObjectException;

/**
 * Created by aiko on 4/1/17.
 */

public interface IUnitOfWork {
    <TEntity extends IDomainObject> TEntity RegisterNew(TEntity entity, InvocationDelegates invocationDelegates)
            throws NoSuchObjectException;

    <TEntity extends IDomainObject> TEntity RegisterDirty(TEntity entity, InvocationDelegates invocationDelegates)
            throws NoSuchObjectException;

    <TEntity extends IDomainObject> TEntity RegisterRemoved(TEntity entity, InvocationDelegates invocationDelegates)
            throws NoSuchObjectException;

    void Commit(UoWInvocationDelegates delegates);
    void ClearUnitOfWork();
    boolean PendingCommits();
}
