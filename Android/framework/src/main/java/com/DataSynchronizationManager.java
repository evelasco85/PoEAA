package com;

import com.Interfaces.IDataSynchronizationManager;
import com.Interfaces.IRepository;
import com.datamanipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.datamanipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.domain.DomainObjectInterfaces.IDomainObject;

import java.lang.reflect.Field;
import java.rmi.NoSuchObjectException;
import java.util.HashMap;
import java.util.List;

/**
 * Created by aiko on 3/4/17.
 */

public class DataSynchronizationManager implements IDataSynchronizationManager{

    static IDataSynchronizationManager s_instance = new DataSynchronizationManager();

    HashMap<String, Object> _serviceContainerDictionary = new HashMap<String, Object>();

    public static IDataSynchronizationManager GetInstance()
    {
        return s_instance;
    }

    public <TEntity extends IDomainObject> void RegisterEntity(Class<TEntity> thisClass, IBaseMapperConcrete<TEntity> mapper, List<IBaseQueryObjectConcrete<TEntity>> queryList)
    {
        EntityServiceContainer<TEntity> serviceContainer = new EntityServiceContainer<TEntity>();

        serviceContainer.Mapper = mapper;
        serviceContainer.IdentityMap = new IdentityMap(thisClass);
        serviceContainer.Repository = new Repository(thisClass, this);
        serviceContainer.QueryDictionary = ConvertQueryListToDictionary(queryList);
        serviceContainer.PrimitiveFields = GetPrimitiveFields(thisClass);


        String key = GetServiceContainerKey(thisClass);

        if (ServiceContainerExists(key))
            _serviceContainerDictionary.remove(key);

        _serviceContainerDictionary.put(key, serviceContainer);
    }

    <TEntity extends IDomainObject>  HashMap<String, IBaseQueryObjectConcrete<TEntity>> ConvertQueryListToDictionary(List<IBaseQueryObjectConcrete<TEntity>> queryList)
    {
        HashMap<String, IBaseQueryObjectConcrete<TEntity>>  queryDictionary = new HashMap<String, IBaseQueryObjectConcrete<TEntity>>();

        if ((queryList == null) || (queryList.size() < 1))
            return queryDictionary;

        for (int index = 0; index < queryList.size(); index++)
        {
            IBaseQueryObjectConcrete<TEntity> query = queryList.get(index);

            if(query == null)
                continue;

            queryDictionary.put(query.GetSearchInputType().getName(), query);
        }

        return queryDictionary;
    }

    <TEntity extends IDomainObject>  HashMap<String, Field> GetPrimitiveFields(Class<TEntity> thisClass)
    {
        HashMap<String, Field> fieldMap = new HashMap<String, Field>();
        Field []fields = thisClass.getDeclaredFields();

        if((fields == null) || (fields.length < 1))
            return fieldMap;

        for(int index = 0; index < fields.length; index++){
            Field field = fields[index];

            if((!field.getType().isAssignableFrom(String.class)) && (!field.getType().isPrimitive()))
                continue;

            fieldMap.put(field.getName(), field);
        }

        return fieldMap;
    }

    <TEntity extends IDomainObject> String GetServiceContainerKey(Class<TEntity> thisClass)
    {
        return thisClass.getName();
    }

    boolean ServiceContainerExists(String key)
    {
        return _serviceContainerDictionary.containsKey(key);
    }

    public <TEntity extends IDomainObject> IRepository<TEntity> GetRepository(Class<TEntity> thisClass)
    {
        EntityServiceContainer<TEntity> container = null;

        try
        {
            container = GetServiceContainer(thisClass);
        }
        catch (NoSuchObjectException ex){
        }

        return container.Repository;
    }

    <TEntity extends IDomainObject> EntityServiceContainer<TEntity> GetServiceContainer(Class<TEntity> thisClass)
            throws NoSuchObjectException
    {
        String key = GetServiceContainerKey(thisClass);

        if (!ServiceContainerExists(key))
            throw new NoSuchObjectException(String.format("Service container with key '%s' not found.", key));

        EntityServiceContainer<TEntity> serviceContainer = (EntityServiceContainer<TEntity>)_serviceContainerDictionary.get(key);

        return serviceContainer;
    }
}
