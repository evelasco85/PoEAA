package com.Interfaces.Registries;

import com.Interfaces.IRepository;
import com.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IRepositoryRegistry {
    <TEntity extends IDomainObject> IRepository<TEntity> GetRepository(Class<TEntity> thisClass);
}
