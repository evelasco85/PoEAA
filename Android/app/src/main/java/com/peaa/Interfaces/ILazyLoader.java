package com.peaa.Interfaces;

/**
 * Created by aiko on 4/2/17.
 */

public interface ILazyLoader<TEntity, TSearchInput>
{
    void LoadAllFields(TEntity entity, TSearchInput criteria);
}
