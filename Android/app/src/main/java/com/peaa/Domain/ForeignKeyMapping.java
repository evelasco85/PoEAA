package com.peaa.Domain;

import com.peaa.Domain.Interfaces.IForeignKeyMapping;

import java.util.List;

/**
 * Created by aiko on 3/4/17.
 */

public abstract class ForeignKeyMapping<TParentEntity, TChildEntity>
        implements IForeignKeyMapping<TParentEntity, TChildEntity>
{
    public List<TChildEntity> GetChildEntities(TParentEntity parent)
    {
        return RetrieveForeignKeyEntities(parent);
    }

    public abstract List<TChildEntity> RetrieveForeignKeyEntities(TParentEntity parent);
}

