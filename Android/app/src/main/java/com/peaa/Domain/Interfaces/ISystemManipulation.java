package com.peaa.Domain.Interfaces;

import com.peaa.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.peaa.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObject;

/**
 * Created by aiko on 3/4/17.
 */

public interface ISystemManipulation {
    void SetQueryObject(IBaseQueryObject queryObject);
    void SetMapper(IBaseMapper mapper);
}
