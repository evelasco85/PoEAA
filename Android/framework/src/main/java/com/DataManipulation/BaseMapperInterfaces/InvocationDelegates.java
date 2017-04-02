package com.DataManipulation.BaseMapperInterfaces;

import com.Domain.Interfaces.IDomainObject;

import java.util.Hashtable;

/**
 * Created by aiko on 3/4/17.
 */

public interface InvocationDelegates {
    Hashtable GetResults();

    void SetResults(Hashtable results);

    void SuccessfulInvocation(IDomainObject domainObject);

    void FailedInvocationDelegate(IDomainObject domainObject);
}
