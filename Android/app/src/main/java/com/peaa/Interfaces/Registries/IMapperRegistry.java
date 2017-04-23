package com.peaa.Interfaces.Registries;

import com.peaa.DataManipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.peaa.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/11/17.
 */

public interface IMapperRegistry {
    <TEntity extends IDomainObject> IBaseMapperConcrete<TEntity> GetMapper(Class<TEntity> thisClass);
}
