package com.Interfaces;

import com.domain.DomainObjectInterfaces.IDomainObject;

import java.util.Hashtable;

/**
 * Created by aiko on 4/1/17.
 */

public interface UoWInvocationDelegates {
    void SuccessfulUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action, Hashtable results);
    void FailedUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action, Hashtable results);
}
