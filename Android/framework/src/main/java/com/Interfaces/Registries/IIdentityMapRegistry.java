package com.Interfaces.Registries;

import com.Interfaces.IIdentityMapConcrete;
import com.Domain.DomainObjectInterfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IIdentityMapRegistry {
    <TEntity extends IDomainObject> IIdentityMapConcrete<TEntity> GetIdentityMap();
}
