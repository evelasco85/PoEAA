package com.peaa.CustomerServices;

import com.peaa.IdentityFieldAnnotation;
import com.peaa.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.peaa.Domain.DomainObject;

/**
 * Created by aiko on 3/5/17.
 */

public class Customer extends DomainObject {
    public Customer(IBaseMapper mapper)
    {
        super(mapper);
    }

    public Customer(IBaseMapper mapper, String number)
    {
        this(mapper);

        Number = number;
    }

    @IdentityFieldAnnotation
    public String Number;

    public String Name;
}
