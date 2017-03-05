package com.datamanipulation.BaseQueryObjectInterfaces;

import java.util.List;

/**
 * Created by aiko on 3/4/17.
 */

public interface IBaseQueryObjectConcrete<TEntity> extends IBaseQueryObject
{
    List<TEntity> Execute();
}