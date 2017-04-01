package com;

import com.Interfaces.IRepository;
import com.datamanipulation.BaseMapperInterfaces.IBaseMapperConcrete;
import com.datamanipulation.BaseQueryObjectInterfaces.IBaseQueryObject;
import com.datamanipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.domain.DomainObjectInterfaces.IDomainObject;
import com.domain.DomainObjectInterfaces.ISystemManipulation;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by aiko on 4/1/17.
 */

public class Repository <TEntity extends IDomainObject> implements IRepository<TEntity>
{
    Class<TEntity> _class;

    public Repository(Class<TEntity> thisClass)
    {
        _class = thisClass;
    }

    void ApplyDomainObjectSettings(List<TEntity> newResult, IBaseQueryObject query)
    {
        if ((newResult == null) || (newResult.size() < 1))
            return;

        IBaseMapperConcrete<TEntity> mapper = DataSynchronizationManager.GetInstance().GetMapper(_class);

        for(int index = 0; index < newResult.size(); ++index)
        {
            TEntity entity = newResult.get(index);

            ((ISystemManipulation)entity).SetQueryObject(query);
            ((ISystemManipulation)entity).SetMapper(mapper);
        }
    }

    List<TEntity> Matching(IBaseQueryObjectConcrete<TEntity> query)
    {
        List<TEntity> results = query.Execute();

        ApplyDomainObjectSettings(results, query);

        return results;
    }

    public <TSearchInput> List<TEntity> Matching(TSearchInput criteria)
    {
        List<TEntity> result = new ArrayList<TEntity>();

        return result;
    }
}
