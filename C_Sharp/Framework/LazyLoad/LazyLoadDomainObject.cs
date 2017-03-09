using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework.LazyLoad
{
    public interface ILazyLoadDomainObject<TSearchInput>
    {
        TSearchInput Criteria { get; }
        void Load<TEntity>(ILazyLoader<TEntity, TSearchInput> loader)
           where TEntity : LazyLoadDomainObject<TSearchInput>;
    }

    public abstract class LazyLoadDomainObject<TSearchInput> : DomainObject, ILazyLoadDomainObject<TSearchInput>
    {
        private TSearchInput _criteria;

        public TSearchInput Criteria { get { return _criteria; } }

        public LazyLoadDomainObject(
            TSearchInput criteria,
            IBaseMapper mapper
            ) : base(mapper)
        {
            _criteria = criteria;
        }

        public void Load<TEntity>(ILazyLoader<TEntity, TSearchInput> loader)
            where TEntity : LazyLoadDomainObject<TSearchInput>
        {
            if ((_criteria == null) || (loader == null))
                return;

            TEntity currentEntity = (TEntity) this;

            loader.LoadAllFields(ref currentEntity, _criteria);

            _criteria = default(TSearchInput);      //Clear criteria
        }
    }
}
