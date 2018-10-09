using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;

namespace Framework.Tests.CustomerServices
{
    using TEntity = Customer;
    using TOutput = Customer;

    public class GetCustomerByIdQuery : BaseQueryObject<TEntity, TOutput, GetCustomerByIdQuery.Criteria>
    {
        public class Criteria : ICriteriaTag<TEntity, TOutput>
        {
            public int CustomerId { get; set; }

            private Criteria()
            {
            }

            public static Criteria SearchById(int id)
            {
                return new Criteria {CustomerId = id};
            }
        }

        public override IBaseMapper<TEntity> GetMapper(IDomainObjectManager manager)
        {
            if (manager == null) return null;

            return manager.GetMapper<TEntity>();
        }

        public override TOutput PerformSearchOperation(IBaseMapper mapper, Criteria searchInput)
        {
            Dictionary<int, string> customerList = new Dictionary<int, string>();

            customerList.Add(1, "Juan dela Cruz");
            customerList.Add(2, "Jane Doe");
            customerList.Add(3, "John Doe");

            Tuple<string, string> searchResult = new Tuple<string, string>(searchInput.CustomerId.ToString(), customerList[searchInput.CustomerId]);
            TOutput customer = new Customer(mapper, this);

            customer.Number = searchResult.Item1;
            customer.Name = searchResult.Item2;

            return customer;
        }
    }
}
