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
    public class DomainObjectTests
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
        public void TestDomainObjectStates()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();

            /*Match by civil status*/
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = GetCustomerByCivilStatusQuery.Criteria.SearchById(GetCustomerByCivilStatusQuery.CivilStatus.Married);
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            List<Customer> results = new List<Customer>();

            results.AddRange(repository.Matching(criteriaByStatus));
            results.AddRange(repository.Matching(criteriaById));

            /*Entities loaded from repository will be marked as 'Clean'*/
            results.ForEach(customer =>
            {
                Assert.AreEqual(DomainObjectState.Clean, customer.GetCurrentState(), "Entities loaded from data source or repository should be marked as 'Clean'");
            });

            /*Entities manually created will be maked as 'Manually_Created'*/
            IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper<Customer>();
            Customer newCustomer = new Customer(mapper);

            Assert.AreEqual(DomainObjectState.Manually_Created, newCustomer.GetCurrentState(), "Entities manually created should be marked as 'Manually_Created'");

            /*Entities where 'MarkAsDirty()' is invoked will be maked as 'Dirty'*/
            newCustomer.MarkAsDirty();

            Assert.AreEqual(DomainObjectState.Dirty, newCustomer.GetCurrentState(), "Entities where 'MarkAsDirty()' is invoked should be marked as 'Dirty'");

            /*Entities where 'MarkForDeletion()' is invoked will be maked as 'For_DataSource_Deletion'*/
            newCustomer.MarkForDeletion();

            Assert.AreEqual(DomainObjectState.For_DataSource_Deletion, newCustomer.GetCurrentState(), "Entities where 'MarkForDeletion()' is invoked should be marked as 'For_DataSource_Deletion'");
        }
    }
}
