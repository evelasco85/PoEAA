package com.DataManipulation.BaseMapperInterfaces;

import com.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/4/17.
 */

public interface IBaseMapperConcrete<TEntity extends IDomainObject> extends IBaseMapper {
    boolean ConcreteUpdate(TEntity entity, InvocationDelegates invocationDelegates);
    boolean ConcreteInsert(TEntity entity, InvocationDelegates invocationDelegates);
    boolean ConcreteDelete(TEntity entity, InvocationDelegates invocationDelegates);
}