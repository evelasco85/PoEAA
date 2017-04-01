package com.Interfaces.Registries;

import com.datamanipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.domain.DomainObjectInterfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IMapperRegistry {
    <TEntity extends IDomainObject> IBaseMapperConcrete<TEntity> GetMapper(Class<TEntity> thisClass);
}
