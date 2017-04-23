package com.peaa.Domain.Interfaces;

import com.peaa.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.peaa.Domain.Enums.InstantiationType;

import java.util.UUID;

/**
 * Created by aiko on 2/11/17.
 */

public interface IDomainObject {
    UUID GetSystemId();
    IBaseMapper GetMapper();
    InstantiationType GetInstantiation();
}


