package com.DataManipulation.BaseQueryObjectInterfaces;

import java.util.List;

/**
 * Created by aiko on 3/4/17.
 */

public interface IBaseQueryObjectSearchableConcrete<TEntity, TSearchInput> extends IBaseQueryObjectConcrete<TEntity> {
    TSearchInput GetSearchInput();
    void SetSearchInput(TSearchInput searchInput);
    List<TEntity> PerformSearchOperation(TSearchInput searchInput);
}