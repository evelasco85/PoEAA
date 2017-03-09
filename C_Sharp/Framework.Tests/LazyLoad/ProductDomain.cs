using Framework.Data_Manipulation;
using Framework.LazyLoad;

namespace Framework.Tests.LazyLoad
{
    public class ProductDomain : LazyLoadDomainObject<ProductDomain.Criteria>
    {
        public class Criteria
        {
            public int Id { get; set; }

            public static Criteria SearchById(int id)
            {
                return new Criteria {Id = id};
            }
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public ProductDomain(IBaseMapper mapper, Criteria criteria)
            : base(criteria, mapper)
        {
            if (criteria != null)
                Id = criteria.Id;
        }
    }
}
