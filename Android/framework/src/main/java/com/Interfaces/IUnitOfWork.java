package com.Interfaces;

import com.domain.DomainObjectInterfaces.IDomainObject;

/**
 * Created by aiko on 4/1/17.
 */

public interface IUnitOfWork {
    <TEntity extends IDomainObject> TEntity RegisterNew(TEntity entity);
    <TEntity extends IDomainObject> TEntity RegisterDirty(TEntity entity);
    <TEntity extends IDomainObject> TEntity RegisterRemoved(TEntity entity);
    void Commit(UoWInvocationDelegates delegates);
    void ClearUnitOfWork();
    boolean PendingCommits();
}
