package com.DataManipulation;

import com.DataManipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.DataManipulation.BaseMapperInterfaces.InvocationDelegates;
import com.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 2/11/17.
 */

public abstract class BaseMapper<TEntity extends IDomainObject> implements IBaseMapperConcrete<TEntity>
{
    final Class<TEntity> _entityClass;

    public BaseMapper(Class<TEntity> entityClass)
    {
        _entityClass = entityClass;
    }

    public String GetEntityTypeName()
    {
        return _entityClass.getName();
    }

    abstract public boolean ConcreteUpdate(TEntity entity, InvocationDelegates invocationDelegates);
    abstract public boolean ConcreteInsert(TEntity entity, InvocationDelegates invocationDelegates);
    abstract public boolean ConcreteDelete(TEntity entity, InvocationDelegates invocationDelegates);

    public boolean Update(IDomainObject entity, InvocationDelegates invocationDelegates)
    {
        TEntity instance = (TEntity) entity;

        return ConcreteUpdate(instance, invocationDelegates);
    }

    public boolean Insert(IDomainObject entity, InvocationDelegates invocationDelegates)
    {
        TEntity instance = (TEntity) entity;

        return ConcreteInsert(instance, invocationDelegates);
    }

    public boolean Delete(IDomainObject entity, InvocationDelegates invocationDelegates)
    {
        TEntity instance = (TEntity) entity;

        return ConcreteDelete(instance, invocationDelegates);
    }
}