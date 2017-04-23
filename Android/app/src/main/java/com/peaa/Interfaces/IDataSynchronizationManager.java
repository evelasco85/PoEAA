package com.peaa.Interfaces;

import com.peaa.Interfaces.Registries.IMapperRegistry;
import com.peaa.Interfaces.Registries.IQueryObjectRegistry;
import com.peaa.Interfaces.Registries.IRepositoryRegistry;
import com.peaa.DataManipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.peaa.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.peaa.Domain.Interfaces.IDomainObject;

import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.List;

/**
 * Created by aiko on 3/11/17.
 */

public interface IDataSynchronizationManager extends IMapperRegistry, IRepositoryRegistry, IQueryObjectRegistry
{
    <TEntity extends IDomainObject> void RegisterEntity(Class<TEntity> thisClass, IBaseMapperConcrete<TEntity> mapper, List<IBaseQueryObjectConcrete<TEntity>> queryList);
    <TEntity extends IDomainObject> HashMap<String, Field> GetFields(Class<TEntity> thisClass);
}
