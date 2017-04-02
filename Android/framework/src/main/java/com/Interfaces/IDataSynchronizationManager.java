package com.Interfaces;

import com.Interfaces.Registries.IMapperRegistry;
import com.Interfaces.Registries.IQueryObjectRegistry;
import com.Interfaces.Registries.IRepositoryRegistry;
import com.DataManipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.Domain.DomainObjectInterfaces.IDomainObject;

import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.List;

/**
 * Created by aiko on 3/11/17.
 */

public interface IDataSynchronizationManager extends IMapperRegistry, IRepositoryRegistry, IQueryObjectRegistry
{
    <TEntity extends IDomainObject> void RegisterEntity(Class<TEntity> thisClass, IBaseMapperConcrete<TEntity> mapper, List<IBaseQueryObjectConcrete<TEntity>> queryList);
    <TEntity  extends IDomainObject> HashMap<String, Field> GetFields(Class<TEntity> thisClass);
}
