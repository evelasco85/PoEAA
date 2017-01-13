using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;
using Framework.Domain;
using Framework.Tests.CustomerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        private IDataSynchronizationManager _manager;
        private CustomerMapper _mapper = new CustomerMapper();

        [TestInitialize]
        public void Initialize()
        {
            _manager = DataSynchronizationManager.GetInstance();

            _manager.RegisterEntity(
                _mapper,
                new List<IBaseQueryObject<Customer>> {
                    {new GetCustomerByIdQuery()},
                    {new GetCustomerByCivilStatusQuery()}
                });
        }

        [TestMethod]
        public void TestUnitOfWork()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();

            /*Match by civil status*/
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = GetCustomerByCivilStatusQuery.Criteria.SearchById(GetCustomerByCivilStatusQuery.CivilStatus.Married);
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            List<Customer> results = new List<Customer>();
            
            results.AddRange(repository.Matching(criteriaByStatus));
            results.AddRange(repository.Matching(criteriaById));

            results.ForEach(customer =>
            {
                Assert.AreEqual(DomainObjectState.Clean, customer.GetCurrentState(), "Data loaded from data source or repository should be marked as 'clean'");
            });
        }
    }
}
