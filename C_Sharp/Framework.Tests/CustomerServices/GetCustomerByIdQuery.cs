using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;

namespace Framework.Tests.CustomerServices
{
    class GetCustomerByIdQuery : BaseQueryObject<Customer, GetCustomerByIdQuery.Criteria>
    {
        public class Criteria
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

        public override IList<Customer> PerformSearchOperation(Criteria searchInput)
        {
            Dictionary<int, string> customerList = new Dictionary<int, string>();

            customerList.Add(1, "Juan dela Cruz");
            customerList.Add(2, "Jane Doe");
            customerList.Add(3, "John Doe");

            Tuple<string, string> searchResult = new Tuple<string, string>(searchInput.CustomerId.ToString(), customerList[searchInput.CustomerId]);
            IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper<Customer>();
            Customer customer = new Customer(mapper);

            customer.Number = searchResult.Item1;
            customer.Name = searchResult.Item2;

            return new List<Customer>(new[] { customer });
        }
    }
}
