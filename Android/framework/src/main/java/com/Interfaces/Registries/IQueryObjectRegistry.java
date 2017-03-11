package com.Interfaces.Registries;

import com.datamanipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.domain.DomainObjectInterfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IQueryObjectRegistry {
    <TEntity extends IDomainObject, TSearchInput> IBaseQueryObjectConcrete<TEntity> GetQueryBySearchCriteria();
}
