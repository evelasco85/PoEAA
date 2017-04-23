package com.peaa.Interfaces.Registries;

import com.peaa.Interfaces.IRepository;
import com.peaa.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IRepositoryRegistry {
    <TEntity extends IDomainObject> IRepository<TEntity> GetRepository(Class<TEntity> thisClass);
}
