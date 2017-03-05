package com.domain.DomainObjectInterfaces;

import com.datamanipulation.BaseMapperInterfaces.IBaseMapper;
import com.datamanipulation.BaseQueryObjectInterfaces.IBaseQueryObject;

/**
 * Created by aiko on 3/4/17.
 */

public interface ISystemManipulation {
    void SetQueryObject(IBaseQueryObject queryObject);
    void SetMapper(IBaseMapper mapper);
}
