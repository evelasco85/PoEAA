using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IEntityServiceContainer<TEntity>
        where TEntity : IDomainObject
    {
        IBaseMapper<TEntity> Mapper { get; set; }
        IRepository<TEntity> Repository { get; set; }
        IDictionary<string, IBaseQueryObject<TEntity>> QueryDictionary { get; set; }
        IIdentityMap<TEntity> IdentityMap { get; set; }
    }

    public class EntityServiceContainer<TEntity> : IEntityServiceContainer<TEntity>
        where TEntity : IDomainObject
    {
        public IBaseMapper<TEntity> Mapper { get; set; }
        public IRepository<TEntity> Repository { get; set; }
        public IDictionary<string, IBaseQueryObject<TEntity>> QueryDictionary { get; set; }
        public IIdentityMap<TEntity> IdentityMap { get; set; }
    }
}
