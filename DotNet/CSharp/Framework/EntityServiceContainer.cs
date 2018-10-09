using System.Collections.Generic;
using System.Reflection;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IEntityServiceContainer<TEntity>
    {
        IBaseMapper<TEntity> Mapper { get; set; }
        IRepository<TEntity> Repository { get; set; }
        IDictionary<string, IBaseQueryObject<TEntity>> QueryDictionary { get; set; }
        IDictionary<string, PropertyInfo> PrimitiveProperties { get; set; }
    }

    public class EntityServiceContainer<TEntity> : IEntityServiceContainer<TEntity>
    {
        public IBaseMapper<TEntity> Mapper { get; set; }
        public IRepository<TEntity> Repository { get; set; }
        public IDictionary<string, IBaseQueryObject<TEntity>> QueryDictionary { get; set; }
        public IDictionary<string, PropertyInfo> PrimitiveProperties {get; set;}
    }
}
