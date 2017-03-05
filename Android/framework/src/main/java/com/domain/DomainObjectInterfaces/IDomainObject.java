package com.domain.DomainObjectInterfaces;

import com.datamanipulation.BaseMapperInterfaces.IBaseMapper;
import com.domain.Enums.InstantiationType;

import java.util.UUID;

/**
 * Created by aiko on 2/11/17.
 */

public interface IDomainObject {
    UUID GetSystemId();
    IBaseMapper GetMapper();
    InstantiationType GetInstantiation();
}


