using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IRepository<TEntity>
        where TEntity : IDomainObject
    {
        IList<TEntity> Matching(IBaseQueryObject<TEntity> query);
    }

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : IDomainObject
    {
        private readonly IDataSynchronizationManager _manager;

        public Repository(IDataSynchronizationManager manager)
        {
            _manager = manager;
        }
        
        //Given the 'query' instance, retrieve all in-memory records resulted from previously invoked 'query' if there are any
        IList<TEntity> GetInMemoryEntities(IBaseQueryObject<TEntity> query)
        {
            throw new NotImplementedException();
        }

        void ConsolidateResultsInMemory(IList<TEntity> newResult)
        {
            if ((newResult == null) || (!newResult.Any()))
                return;
         
            ((List<TEntity>)_manager.GetLoadedEntities<TEntity>()).AddRange(newResult);
        }

        public IList<TEntity> Matching(IBaseQueryObject<TEntity> query)
        {
            IList<TEntity> inMemoryEntities = GetInMemoryEntities(query);

            if ((inMemoryEntities == null) || (!inMemoryEntities.Any()))
            {
                inMemoryEntities = query.Execute();

                ConsolidateResultsInMemory(inMemoryEntities);
            }

            return inMemoryEntities;
        }
    }
}
