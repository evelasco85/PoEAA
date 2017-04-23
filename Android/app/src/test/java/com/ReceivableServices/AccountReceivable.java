package com.ReceivableServices;

import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.Domain.DomainObject;
import com.IdentityFieldAnnotation;

/**
 * Created by aiko on 4/8/17.
 */

public class AccountReceivable extends DomainObject {

    @IdentityFieldAnnotation
    public String Number;

    public String CustomerNumber ;

    public String Description;

    public AccountReceivable(String customerNumber, String number, IBaseMapper mapper)
    {
        super(mapper);

        CustomerNumber = customerNumber;
        Number = number;
    }
}
