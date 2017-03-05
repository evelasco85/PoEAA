package com.datamanipulation.BaseMapperInterfaces;

import com.domain.DomainObjectInterfaces.IDomainObject;

/**
 * Created by aiko on 3/4/17.
 */

public interface InvocationDelegates
{
    void SuccessfulInvocation(IDomainObject domainObject, Object additionalInfo);
    void FailedInvocationDelegate(IDomainObject domainObject, Exception exception, Object additionalInfo);
}
