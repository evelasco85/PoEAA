using Framework.Domain;

namespace Framework.Data_Manipulation
{
    public interface IBaseQueryObject<TEntity>
        where TEntity : IDomainObject
    {
        
    }

    //Builder pattern
    public class BaseQueryObject<TEntity> : IBaseQueryObject<TEntity>
        where TEntity : IDomainObject
    {
    }
}
