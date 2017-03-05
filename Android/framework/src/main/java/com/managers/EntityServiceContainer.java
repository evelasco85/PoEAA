package com.managers;

import com.datamanipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.datamanipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.domain.DomainObjectInterfaces.IDomainObject;
import com.managers.EntityService.IEntityServiceContainer;

import java.lang.reflect.Field;
import java.util.HashMap;

/**
 * Created by aiko on 2/11/17.
 */

//public class EntityServiceContainer<TEntity extends IDomainObject> implements IEntityServiceContainer<TEntity> {
//    IBaseMapperConcrete<TEntity> _mapper;
//    IBaseQueryObjectConcrete<TEntity> _queryObject;
//    HashMap<String, Field> _primitiveProperties;
//
//    @Override
//    public IBaseMapperConcrete<TEntity> GetMapper() {
//        return _mapper;
//    }
//
//    @Override
//    public void SetMapper(IBaseMapperConcrete<TEntity> mapper) {
//        _mapper = mapper;
//    }
//
//    @Override
//    public IBaseQueryObjectConcrete<TEntity> GetQueryObject() {
//        return _queryObject;
//    }
//
//    @Override
//    public void SetQueryObject(IBaseQueryObjectConcrete<TEntity> queryObject) {
//        _queryObject = queryObject;
//    }
//
//    @Override
//    public HashMap<String, Field> GetPrimitiveProperties() {
//        return _primitiveProperties;
//    }
//
//    @Override
//    public void SetPrimitiveProperties(HashMap<String, Field> primitiveProperties) {
//        _primitiveProperties = primitiveProperties;
//    }
//}
