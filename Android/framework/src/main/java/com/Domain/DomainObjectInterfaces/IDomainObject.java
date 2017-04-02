package com.Domain.DomainObjectInterfaces;

import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.Domain.Enums.InstantiationType;

import java.util.UUID;

/**
 * Created by aiko on 2/11/17.
 */

public interface IDomainObject {
    UUID GetSystemId();
    IBaseMapper GetMapper();
    InstantiationType GetInstantiation();
}


