package com.peaa.Interfaces.Registries;

import com.peaa.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.peaa.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IQueryObjectRegistry {
    <TEntity extends IDomainObject, TSearchInput> IBaseQueryObjectConcrete<TEntity> GetQueryBySearchCriteria(Class<TEntity> thisClass, Class<TSearchInput> thisSearch);
}
