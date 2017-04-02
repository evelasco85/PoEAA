package com.LazyLoad;

import com.Domain.Interfaces.IDomainObject;
import com.Interfaces.ILazyLoader;

import java.util.ArrayList;

/**
 * Created by aiko on 4/2/17.
 */

public class LazyLoadList<
        TEntity extends LazyLoadDomainObject<TSearchInput>,
        TSearchInput> extends ArrayList<TEntity> {

    ILazyLoader<TEntity, TSearchInput> _loader;

    public LazyLoadList(ILazyLoader<TEntity, TSearchInput> loader)
    {
        _loader = loader;
    }

    @Override
    public TEntity get(int index)
    {
        TEntity entity = super.get(index);

        if ((entity == null) || (_loader == null))
            return entity;

        //Perform complete load
        entity.Load(_loader);

        return entity;
    }
}
