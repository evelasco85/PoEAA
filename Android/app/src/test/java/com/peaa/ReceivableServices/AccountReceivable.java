package com.peaa.ReceivableServices;

import com.peaa.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.peaa.Domain.DomainObject;
import com.peaa.IdentityFieldAnnotation;

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
