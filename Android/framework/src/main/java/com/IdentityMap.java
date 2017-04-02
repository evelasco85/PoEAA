package com;

import com.Interfaces.IIdentityMapConcrete;
import com.Interfaces.IIdentityMapQuery;
import com.Domain.DomainObjectInterfaces.IDomainObject;
import com.Security.HashService;
import com.Security.IHashService;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.UUID;

/**
 * Created by aiko on 3/5/17.
 */

public class IdentityMap<TEntity extends IDomainObject> implements
        IIdentityMapConcrete<TEntity>, IIdentityMapQuery<TEntity> {
    List<Field> _identityFields = new ArrayList<Field>();
    HashMap<UUID, IDomainObject> _guidToDomainObjectDictionary = new HashMap<UUID, IDomainObject>();
    HashMap<String, UUID> _hashToGuidDictionary = new HashMap<String, UUID>();
    HashMap<String, Object> _currentSearchDictionary = new HashMap<String, Object>();
    Class<TEntity> _class;

    public IdentityMap(Class<TEntity> thisClass){
        _class = thisClass;
        _identityFields = GetIdentities();
    }

    @Override
    public int GetCount() {
        return _guidToDomainObjectDictionary.size();
    }

    List<Field> GetIdentities()
    {
        Field []fields = _class.getDeclaredFields();
        List<Field> identityFields = new ArrayList<Field>();

        if((fields == null) || (fields.length < 1))
            return identityFields;

        for(int index = 0; index < fields.length; index++){
            Field field = fields[index];
            IdentityFieldAnnotation identityFieldAnnotation = field.getAnnotation(IdentityFieldAnnotation.class);

            if((identityFieldAnnotation == null) || (!(identityFieldAnnotation instanceof IdentityFieldAnnotation)) )
                continue;

            if((!field.getType().isAssignableFrom(String.class)) && (!field.getType().isPrimitive()))
                continue;

            identityFields.add(field);
        }

        return identityFields;
    }

    public IIdentityMapQuery<TEntity> SearchBy()
    {
        //Start new seach
        _currentSearchDictionary.clear();

        return this;
    }

    public <TEntityPropertyType> IIdentityMapQuery<TEntity>SetFilter(String keyToSearch, Object keyValue)
            throws NoSuchFieldException
    {
        String searchValue = keyValue.toString();
        Boolean found = false;

        for(int index = 0; index < _identityFields.size(); index++){
            if(_identityFields.get(index).getName() == keyToSearch)
            {
                found = true;
                break;
            }
        }

        if(!found)
            throw new NoSuchFieldException(String.format("Field '%s' is not found or not marked with 'IdentityFieldAnnotation'", keyToSearch));

        _currentSearchDictionary.put(keyToSearch, searchValue);

        return this;
    }

    @Override
    public TEntity GetEntity() {
        List<Object> values = GetValuesByFieldOrdinals_Search(_currentSearchDictionary);

        String searchHash = CreateHash(values);
        UUID foundGuid = (_hashToGuidDictionary.containsKey(searchHash)) ? _hashToGuidDictionary.get(searchHash) : null;

        if (foundGuid != null)
            return (TEntity)_guidToDomainObjectDictionary.get(foundGuid);

        return null;
    }

    @Override
    public List<Field> GetIdentityFields() {
        return Collections.unmodifiableList(_identityFields);
    }

    @Override
    public Boolean EntityHasValidIdentityFields() {
        return (_identityFields.size() > 0);
    }

    @Override
    public void ClearEntities() {
        _guidToDomainObjectDictionary.clear();
        _hashToGuidDictionary.clear();
    }

    @Override
    public void AddEntity(TEntity entity)
            throws IllegalArgumentException
    {
        if(!EntityHasValidIdentityFields())
            throw new IllegalArgumentException(String.format("'%s' requires declaration of 'IdentityFieldAnnotation' on primitive field(s) to use IdentityMap",
                    _class.getName()));

        String hash = CreateHash(entity);
        UUID systemId = entity.GetSystemId();

        if (!VerifyEntityExistence(hash, systemId))
        {
            _guidToDomainObjectDictionary.put(systemId, entity);
            _hashToGuidDictionary.put(hash, systemId);
        }
    }

    String CreateHash(TEntity entity)
    {
        List<Object> values = GetValuesByFieldOrdinals_CreateHash(entity);

        return CreateHash(values);
    }

    List<Object> GetValuesByFieldOrdinals_CreateHash(Object entity)
    {
        List<Object> values = new ArrayList<>();

        if (entity == null)
            return values;

        for (int index = 0; index < _identityFields.size(); index++) {
            Object valueObj = null;

            Field field = _identityFields.get(index);

            try
            {
                valueObj = field.get(entity);
            }
            catch (IllegalAccessException ex) {

            }

            if (valueObj == null)
                continue;

            values.add(valueObj);
        }

        return values;
    }

    List<Object> GetValuesByFieldOrdinals_Search(HashMap<String, Object> seachMap)
    {
        List<Object> values = new ArrayList<>();

        if (seachMap == null)
            return values;

        for (int index = 0; index < _identityFields.size(); index++) {
            Object valueObj = null;

            Field field = _identityFields.get(index);

            if (!seachMap.containsKey(field.getName()))
                valueObj = null;
            else
                valueObj = seachMap.get(field.getName());

            if (valueObj == null)
                continue;

            values.add(valueObj);
        }

        return values;
    }

    String CreateHash(List<Object> values)
    {
        StringBuilder hashSet = new StringBuilder();
        IHashService service = HashService.getInstance();

        for (int index = 0; index < values.size(); index++)
        {
            Object valueObj = values.get(index);

            hashSet.append(service.ComputeHashValue(valueObj.toString()));
        }

        String accumulatedHash = service.ComputeHashValue(hashSet.toString());

        return accumulatedHash;
    }

    boolean VerifyEntityExistence(String hash, UUID systemId)
    {
        //Verify hash dictionary
        if ((hash != null) && (hash.length() > 0) && (_hashToGuidDictionary.containsKey(hash)))
            return true;

        //Verify guid dictionary
        if (_guidToDomainObjectDictionary.containsKey(systemId))
        {
            FixDictionaryAnomalies(hash, systemId);

            return true;
        }

        return false;
    }

    void FixDictionaryAnomalies(String hash, UUID systemId)
    {
        //Anomalies found, possibly by change of primary key(s) values
        //Perform reverse-lookup
        List<Map.Entry<String, UUID>> inconsistentHashEntries = new ArrayList<Map.Entry<String, UUID>>();
        Iterator iterator = _hashToGuidDictionary.entrySet().iterator();

        while(iterator.hasNext())
        {
            Map.Entry<String, UUID> pair = (Map.Entry<String, UUID> )iterator.next();

            if(pair.getValue() == systemId)
                inconsistentHashEntries.add(pair);
        }

        //Update/Fix inconsistencies
        if ((inconsistentHashEntries != null) && !inconsistentHashEntries.isEmpty())
        {
            for(int index = 0; index < inconsistentHashEntries.size(); index++)
            {
                Map.Entry<String, UUID> entry = inconsistentHashEntries.get(index);

                _hashToGuidDictionary.remove(entry.getKey());
            }


            _hashToGuidDictionary.put(hash, systemId);
        }
    }
}
