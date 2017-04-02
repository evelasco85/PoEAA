package com;

import com.Interfaces.IIdentityMapConcrete;
import com.Interfaces.IRepository;
import com.DataManipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.Domain.Interfaces.IDomainObject;

import java.lang.reflect.Field;
import java.util.HashMap;

/**
 * Created by aiko on 2/11/17.
 */

public class EntityServiceContainer<TEntity extends IDomainObject> {
    public IBaseMapperConcrete<TEntity> Mapper;
    public IIdentityMapConcrete<TEntity> IdentityMap;
    public IRepository<TEntity> Repository;
    public HashMap<String, IBaseQueryObjectConcrete<TEntity>> QueryDictionary;
    public HashMap<String, Field> PrimitiveFields;
}

