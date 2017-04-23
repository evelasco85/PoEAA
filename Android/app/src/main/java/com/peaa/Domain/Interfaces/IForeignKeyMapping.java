package com.peaa.Domain.Interfaces;

import java.util.List;

/**
 * Created by aiko on 3/4/17.
 */

public interface IForeignKeyMapping<TParentEntity, TChildEntity>
{
    List<TChildEntity> GetChildEntities(TParentEntity parent);
    List<TChildEntity> RetrieveForeignKeyEntities(TParentEntity parent);
}