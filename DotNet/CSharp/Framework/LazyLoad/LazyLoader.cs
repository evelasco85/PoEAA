namespace Framework.LazyLoad
{
    public interface ILazyLoader<TEntity, TSearchInput>
    {
        void LoadAllFields(ref TEntity entity, TSearchInput criteria);
    }

    public abstract class LazyLoader<TEntity, TSearchInput> : ILazyLoader<TEntity, TSearchInput>
        where TEntity : LazyLoadDomainObject<TSearchInput>
    {
        public abstract void LoadAllFields(ref TEntity entity, TSearchInput criteria);
    }
}
