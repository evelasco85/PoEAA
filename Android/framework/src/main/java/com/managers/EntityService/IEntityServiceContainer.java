package com.managers.EntityService;

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

public interface IEntityServiceContainer<TEntity extends IDomainObject>{
    IBaseMapperConcrete<TEntity> GetMapper();
    void SetMapper(IBaseMapperConcrete<TEntity> mapper);

    IRepository<TEntity> GetRepository();
    void SetRepository(IRepository<TEntity> repository);

    HashMap<String, IBaseQueryObjectConcrete<TEntity>> GetQueryDictionary();
    void SetQueryDictionary(HashMap<String, IBaseQueryObjectConcrete<TEntity>> queryDictionary);

    IIdentityMapConcrete<TEntity> GetIdentityMap();
    void SetIdentityMap(IIdentityMapConcrete<TEntity> identityMap);

    HashMap<String, Field> GetPrimitiveProperties();
    void SetPrimitiveProperties(HashMap<String, Field> primitiveProperties);
}
