package com.peaa.Interfaces;

import com.peaa.Domain.Interfaces.IDomainObject;

import java.util.List;

/**
 * Created by aiko on 3/4/17.
 */

public interface IRepository<TEntity extends IDomainObject> {
    <TSearchInput> List<TEntity> Matching(TSearchInput criteria);
}
