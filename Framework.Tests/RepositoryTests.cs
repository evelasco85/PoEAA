using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Framework.Data_Manipulation;
using Framework.Tests.CustomerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    /// <summary>
    /// Summary description for RepositoryTests
    /// </summary>
    [TestClass]
    public class RepositoryTests
    {
        private IDataSynchronizationManager _manager;

        [TestInitialize]
        public void Initialize()
        {
            _manager = DataSynchronizationManager.GetInstance();

            _manager.RegisterEntity(
                new CustomerMapper(), 
                new List<IBaseQueryObject<Customer>> {
                    {new GetCustomerByIdQuery()},
                    {new GetCustomerByCivilStatusQuery()}
                });
        }

        [TestMethod]
        public void TestRepository()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();

            /*Match by civil status*/
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = GetCustomerByCivilStatusQuery.Criteria.SearchById(GetCustomerByCivilStatusQuery.CivilStatus.Married);
            IList<Customer> resultsByStatus = repository.Matching(criteriaByStatus);
            Customer matchByStatus = resultsByStatus.First();

            Assert.AreEqual("5", matchByStatus.Number);
            Assert.AreEqual("Test Married", matchByStatus.Name);
            /***********************/

            /*Match by Id*/
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            IList<Customer> resultsById = repository.Matching(criteriaById);
            Customer matchById = resultsById.First();

            Assert.AreEqual("2", matchById.Number);
            Assert.AreEqual("Jane Doe", matchById.Name);
            /************/
        }
    }
}
