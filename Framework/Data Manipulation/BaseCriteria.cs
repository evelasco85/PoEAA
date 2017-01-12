namespace Framework.Data_Manipulation
{
    public interface IBaseCriteria<TEntity>
    {
        
    }

    //Builder pattern
    public abstract class BaseCriteria<TEntity> : IBaseCriteria<TEntity>
    {
    }
}
