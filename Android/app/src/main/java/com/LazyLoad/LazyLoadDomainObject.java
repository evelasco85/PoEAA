package com.LazyLoad;

import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.Domain.DomainObject;
import com.Interfaces.ILazyLoadDomainObject;
import com.Interfaces.ILazyLoader;

/**
 * Created by aiko on 4/2/17.
 */

public abstract class LazyLoadDomainObject<TSearchInput> extends DomainObject implements ILazyLoadDomainObject<TSearchInput> {
    TSearchInput _criteria;

    public TSearchInput GetCriteria (){return _criteria;}

    public LazyLoadDomainObject(TSearchInput criteria, IBaseMapper mapper) {
        super(mapper);

        _criteria = criteria;
    }

    public <TEntity extends  LazyLoadDomainObject<TSearchInput>> void Load(ILazyLoader<TEntity, TSearchInput> loader)
    {
        if ((_criteria == null) || (loader == null))
            return;

        TEntity currentEntity = (TEntity) this;

        loader.LoadAllFields(currentEntity, _criteria);

        _criteria = null;
    }
}
