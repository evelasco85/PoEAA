package com.Interfaces.Registries;

import com.Interfaces.IRepository;
import com.domain.DomainObjectInterfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IRepositoryRegistry {
    <TEntity extends IDomainObject> IRepository<TEntity> GetRepository(Class<TEntity> thisClass);
}
