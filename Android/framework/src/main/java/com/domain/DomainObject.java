package com.domain;

import com.datamanipulation.BaseMapperInterfaces.IBaseMapper;
import com.datamanipulation.BaseQueryObjectInterfaces.IBaseQueryObject;
import com.domain.DomainObjectInterfaces.IDomainObject;
import com.domain.DomainObjectInterfaces.ISystemManipulation;
import com.domain.Enums.InstantiationType;

import java.util.UUID;

/**
 * Created by aiko on 3/4/17.
 */

public class DomainObject implements IDomainObject, ISystemManipulation{
    IBaseMapper _mapper;
    UUID _systemId;
    IBaseQueryObject _queryObject;

    public DomainObject(IBaseMapper mapper)
    {
        _mapper = mapper;
        _systemId = UUID.randomUUID();
    }

    @Override
    public UUID GetSystemId() {
        return _systemId;
    }

    @Override
    public IBaseMapper GetMapper() {
        return _mapper;
    }

    @Override
    public InstantiationType GetInstantiation() {
        if(_queryObject == null)
            return InstantiationType.New;
        else
            return InstantiationType.Loaded;
    }

    @Override
    public void SetQueryObject(IBaseQueryObject queryObject) {
        _queryObject = queryObject;
    }

    @Override
    public void SetMapper(IBaseMapper mapper) {
        _mapper = mapper;
    }
}
