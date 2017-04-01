package com;

import com.Interfaces.IForeignKeyMappingManager;
import com.domain.DomainObjectInterfaces.IDomainObject;
import com.domain.ForeignKeyMappingInterfaces.IForeignKeyMapping;

import java.rmi.NoSuchObjectException;
import java.util.HashMap;
import java.util.List;

/**
 * Created by aiko on 4/1/17.
 */

public class ForeignKeyMappingManager implements IForeignKeyMappingManager {
    static IForeignKeyMappingManager s_instance = new ForeignKeyMappingManager();

    HashMap<String, HashMap<String, Object>> _foreignKeMapping = new HashMap<String, HashMap<String, Object>>();

    private ForeignKeyMappingManager(){}

    public static IForeignKeyMappingManager GetInstance()
    {
        return s_instance;
    }

    public <TParentEntity extends IDomainObject, TChildEntity extends IDomainObject>
    void RegisterForeignKeyMapping(Class<TParentEntity> parentClass, Class<TChildEntity> childClass, IForeignKeyMapping<TParentEntity, TChildEntity> mapping)
    {
        String parentName = parentClass.getName();

        if (!_foreignKeMapping.containsKey(parentName))
            _foreignKeMapping.put(parentName, new HashMap<String, Object>());

        HashMap<String, Object> maps = _foreignKeMapping.get(parentName);
        String childName = childClass.getName();

        if (!maps.containsKey(childName))
            maps.put(childName, mapping);
    }

    public <TParentEntity extends IDomainObject, TChildEntity extends IDomainObject>
    List<TChildEntity> GetForeignKeyValues(Class<TParentEntity> parentClass, Class<TChildEntity> childClass, TParentEntity entity)
            throws NoSuchObjectException
    {
        String parentName = parentClass.getName();

        if (!_foreignKeMapping.containsKey(parentName))
            throw new NoSuchObjectException(String.format("Foreign key mapping associated to parent type '%s' does not exists", parentName));

        HashMap<String, Object> maps = _foreignKeMapping.get(parentName);
        String childName = childClass.getName();

        if (!maps.containsKey(childName))
            throw new NoSuchObjectException(String.format("Foreign key mapping associated to child type '%s' does not exists", childName));

        IForeignKeyMapping<TParentEntity, TChildEntity> fkMapping = (IForeignKeyMapping<TParentEntity, TChildEntity>)maps.get(childName);

        return fkMapping.GetChildEntities(entity);
    }
}
