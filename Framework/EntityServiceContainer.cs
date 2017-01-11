using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IEntityServiceContainer<TEntity>
        where TEntity : IDomainObject
    {
        IList<TEntity> LoadedEntities { get; set; }
        IBaseMapper<TEntity> Mapper { get; set; }
        IBaseQueryObject<TEntity> QueryObject { get; set; }
        IBaseRepository<TEntity> Repository { get; set; }
    }

    public class EntityServiceContainer<TEntity> : IEntityServiceContainer<TEntity>
        where TEntity : IDomainObject
    {
        public IList<TEntity> LoadedEntities { get; set; }
        public IBaseMapper<TEntity> Mapper { get; set; }
        public IBaseQueryObject<TEntity> QueryObject { get; set; }
        public IBaseRepository<TEntity> Repository { get; set; }
    }
}
