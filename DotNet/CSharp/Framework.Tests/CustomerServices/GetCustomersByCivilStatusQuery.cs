using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Data_Manipulation;
using static Framework.Tests.CustomerServices.GetCustomersByCivilStatusQuery.Criteria;

namespace Framework.Tests.CustomerServices
{
    using TEntity = Customer;
    using TOutput = IList<Customer>;

    public class GetCustomersByCivilStatusQuery : BaseQueryObject<
        TEntity,
        TOutput,
        GetCustomersByCivilStatusQuery.Criteria
        >
    {
        public class Criteria : ICriteriaTag<TEntity, TOutput>
        {
            public enum CivilStatus
            {
                Single,
                Married,
                Divorced
            }

            public CivilStatus CurrentCivilStatus { get; set; }

            private Criteria() { }

            public static Criteria SearchByStatus(CivilStatus status)
            {
                return new Criteria { CurrentCivilStatus = status };
            }
        }

        public override IBaseMapper<TEntity> GetMapper(IDomainObjectManager manager)
        {
            if (manager == null) return null;

            return manager.GetMapper<TEntity>();
        }

        public override IList<TEntity> PerformSearchOperation(IBaseMapper mapper, Criteria searchInput)
        {
            IList<Tuple<string, string, CivilStatus>> customerList = new List<Tuple<string, string, CivilStatus>>();
            
            customerList.Add(new Tuple<string, string, CivilStatus>("4", "Test Single", CivilStatus.Single));
            customerList.Add(new Tuple<string, string, CivilStatus>("5", "Test Married", CivilStatus.Married));
            customerList.Add(new Tuple<string, string, CivilStatus>("6", "Test Divorced", CivilStatus.Divorced));
            customerList.Add(new Tuple<string, string, CivilStatus>("7", "Test Married", CivilStatus.Married));

            return customerList
                .Where(customer => customer.Item3 == searchInput.CurrentCivilStatus)
                .Select(customer => new Customer(mapper, this)
                {
                    Number = customer.Item1,
                    Name = customer.Item2
                })
                .ToList();
        }
    }
}
