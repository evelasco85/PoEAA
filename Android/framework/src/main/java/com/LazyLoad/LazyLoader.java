package com.LazyLoad;

import com.Interfaces.ILazyLoader;

/**
 * Created by aiko on 4/2/17.
 */

public abstract class LazyLoader <TSearchInput, TEntity extends LazyLoadDomainObject<TSearchInput>> implements ILazyLoader<TEntity, TSearchInput> {
    public abstract void LoadAllFields(TEntity entity, TSearchInput criteria);
}
