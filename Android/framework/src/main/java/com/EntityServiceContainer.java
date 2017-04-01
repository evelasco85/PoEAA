package com;

import com.Interfaces.IIdentityMapConcrete;
import com.Interfaces.IRepository;
import com.datamanipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.datamanipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.domain.DomainObjectInterfaces.IDomainObject;

import java.lang.reflect.Field;
import java.util.HashMap;

/**
 * Created by aiko on 2/11/17.
 */

public class EntityServiceContainer<TEntity extends IDomainObject> {
    Class<TEntity> _class;

    public EntityServiceContainer(Class<TEntity> thisClass)
    {
        _class = thisClass;
    }

    public IBaseMapperConcrete<TEntity> Mapper;

    public IRepository<TEntity> Repository;

    public HashMap<String, IBaseQueryObjectConcrete<TEntity>> QueryDictionary = new HashMap<String, IBaseQueryObjectConcrete<TEntity>>();

    public IIdentityMapConcrete<TEntity> IdentityMap = new IdentityMap<TEntity>(_class);

    public HashMap<String, Field> PrimitiveProperties ;
}

