package com.Domain.Interfaces;

import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObject;

/**
 * Created by aiko on 3/4/17.
 */

public interface ISystemManipulation {
    void SetQueryObject(IBaseQueryObject queryObject);
    void SetMapper(IBaseMapper mapper);
}
