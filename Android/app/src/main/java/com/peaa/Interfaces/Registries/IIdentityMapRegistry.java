package com.peaa.Interfaces.Registries;

import com.peaa.Interfaces.IIdentityMapConcrete;
import com.peaa.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IIdentityMapRegistry {
    <TEntity extends IDomainObject> IIdentityMapConcrete<TEntity> GetIdentityMap();
}
