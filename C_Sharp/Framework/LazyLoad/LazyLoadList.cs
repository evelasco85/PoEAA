using System.Collections.Generic;
using Framework.Domain;

namespace Framework.LazyLoad
{
    public class LazyLoadList<TEntity, TSearchInput> : List<TEntity>, IList<TEntity>
        where TEntity : LazyLoadDomainObject<TSearchInput>, IDomainObject
    {
        private ILazyLoader<TEntity, TSearchInput> _loader;

        public LazyLoadList(ILazyLoader<TEntity, TSearchInput> loader)
        {
            _loader = loader;
        }

        
        public new TEntity this[int index]
        {
            get
            {
                TEntity entity = base[index];

                if ((entity == null) || (entity.Criteria == null) || (_loader == null))
                    return entity;

                //Perform complete load
                entity.Load(_loader, entity.Criteria);

                return entity;
            }
            set { base[index] = value; }
        }
    }
}
