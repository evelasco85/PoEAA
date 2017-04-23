package com.DataManipulation.BaseMapperInterfaces;

import com.Domain.Interfaces.IDomainObject;

/**
 * Created by aiko on 3/4/17.
 */

public interface IBaseMapper
{
    String GetEntityTypeName();
    boolean Update(IDomainObject entity, InvocationDelegates invocationDelegates);
    boolean Insert(IDomainObject entity, InvocationDelegates invocationDelegates);
    boolean Delete(IDomainObject entity, InvocationDelegates invocationDelegates);
}
