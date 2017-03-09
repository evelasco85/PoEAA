using Framework.Data_Manipulation;

namespace Framework.LazyLoad
{
    public delegate TEntity RetrieveMatchingEntityDelegate<TEntity>();

    public interface ILazyLoader
    {
        IBaseMapper Mapper { get; }    
    }

    public interface ILazyLoader<TEntity, TSearchInput> : ILazyLoader
    {
        TEntity GetEntity(TSearchInput criteria);
        void LoadAllFields(ref TEntity entity, RetrieveMatchingEntityDelegate<TEntity> retrieveMatchingEntity);
    }

    public abstract class LazyLoader<TEntity, TSearchInput> : ILazyLoader<TEntity, TSearchInput>
        where TEntity : LazyLoadDomainObject<TSearchInput>
    {
        private IBaseMapper _mapper;

        public IBaseMapper Mapper { get { return _mapper; } }

        public LazyLoader(IBaseMapper mapper)
        {
            _mapper = mapper;
        }

        public abstract void LoadAllFields(ref TEntity entity, RetrieveMatchingEntityDelegate<TEntity> retrieveMatchingEntity);
        public abstract TEntity GetEntity(TSearchInput criteria);
    }
}
