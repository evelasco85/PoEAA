package com.DataManipulation;

import com.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObjectSearchableConcrete;
import java.util.List;

/**
 * Created by aiko on 2/11/17.
 */

public abstract class BaseQueryObject<TEntity, TSearchInput>
        implements IBaseQueryObjectSearchableConcrete<TEntity, TSearchInput> {
    TSearchInput _searchInput;
    Class _searchInputType;

    public TSearchInput GetSearchInput() {
        return _searchInput;
    }

    public void SetSearchInput(TSearchInput searchInput) {
        _searchInput = searchInput;
    }

    public Object GetSearchInputObject() {
        return _searchInput;
    }

    public void SetSearchInputObject(Object inputObject) {
        _searchInput = (TSearchInput) inputObject;
    }

    public Class GetSearchInputType() {
        return _searchInputType;
    }

    public BaseQueryObject(Class searchInputType) {
        _searchInputType = searchInputType;
    }

    public abstract List<TEntity> PerformSearchOperation(TSearchInput searchInput);

    public List<TEntity> Execute() {
        List<TEntity> searchResult = PerformSearchOperation(_searchInput);

        return searchResult;
    }
}
