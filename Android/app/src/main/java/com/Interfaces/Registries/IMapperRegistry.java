package com.Interfaces.Registries;

import com.DataManipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IMapperRegistry {
    <TEntity extends IDomainObject> IBaseMapperConcrete<TEntity> GetMapper(Class<TEntity> thisClass);
}
