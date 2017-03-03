using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;

namespace Framework.Tests.CustomerServices
{
    public class GetCustomerByCivilStatusQuery : BaseQueryObject<Customer, GetCustomerByCivilStatusQuery.Criteria>
    {
        public enum CivilStatus
        {
            Single,
            Married,
            Divorced
        }

        public class Criteria
        {
            public CivilStatus CurrentCivilStatus { get; set; }

            private Criteria()
            {
            }

            public static Criteria SearchByStatus(CivilStatus status)
            {
                return new Criteria { CurrentCivilStatus = status };
            }
        }

        public override IList<Customer> PerformSearchOperation(Criteria searchInput)
        {
            Dictionary<CivilStatus, Tuple<string, string>> customerList = new Dictionary<CivilStatus, Tuple<string, string>>();

            
            customerList.Add(CivilStatus.Single, new Tuple<string, string>("4", "Test Single"));
            customerList.Add(CivilStatus.Married, new Tuple<string, string>("5", "Test Married"));
            customerList.Add(CivilStatus.Divorced, new Tuple<string, string>("6", "Test Divorced"));

            Tuple<string, string> searchResult = customerList[searchInput.CurrentCivilStatus];
            IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper<Customer>();
            Customer customer = new Customer(mapper);

            customer.Number = searchResult.Item1;
            customer.Name = searchResult.Item2;

            return new List<Customer>(new[] { customer });
        }
    }
}
