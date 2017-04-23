package com.peaa.Interfaces;

import com.peaa.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/4/17.
 */

public interface IIdentityMapConcrete<TEntity extends IDomainObject> extends IIdentityMap {
    void AddEntity(TEntity entity) throws IllegalArgumentException;
    IIdentityMapQuery<TEntity> SearchBy();
}