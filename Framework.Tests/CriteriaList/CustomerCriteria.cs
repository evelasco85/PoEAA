using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Tests.Entities;

namespace Framework.Tests.CriteriaList
{
    class GetCustomerByIdCriteria  : BaseCriteria<Customer, int, Tuple<string, string>>
    {
        public override Tuple<string, string> PerformSearchOperation(int searchInput)
        {
            Dictionary<int, string> customerList = new Dictionary<int, string>();

            customerList.Add(1, "Juan dela Cruz");
            customerList.Add(2, "Jane Doe");
            customerList.Add(3, "John Doe");

            return new Tuple<string, string>(searchInput.ToString(), customerList[searchInput]);
        }

        public override IList<Customer> ConstructOutput(Tuple<string, string> searchResult, IBaseMapper_Instantiator<Customer> mapper)
        {
            Customer customer = mapper.CreateEntity();

            customer.Number = searchResult.Item1;
            customer.Name = searchResult.Item2;

            return new List<Customer>(new[] { customer });
        }
    }
}
