using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Tests.CustomerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Framework.Tests.CustomerServices.GetCustomersByCivilStatusQuery.Criteria;

namespace Framework.Tests
{
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
                    {new GetCustomersByCivilStatusQuery()}
                });
        }

        [TestMethod]
        public void TestRepository()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();

            /*Match by civil status*/
            GetCustomersByCivilStatusQuery.Criteria criteriaByStatus = GetCustomersByCivilStatusQuery.Criteria.SearchByStatus(CivilStatus.Married);
            IList<Customer> resultsByStatus = repository.Matching(criteriaByStatus);

            Assert.AreEqual(2, resultsByStatus.Count);
            Assert.AreEqual("5", resultsByStatus[0].Number);
            Assert.AreEqual("Test Married", resultsByStatus[0].Name);
            Assert.AreEqual("7", resultsByStatus[1].Number);
            Assert.AreEqual("Test Married", resultsByStatus[1].Name);
            /***********************/

            /*Match by Id*/
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            Customer matchById = repository.Matching(criteriaById);

            Assert.AreEqual("2", matchById.Number);
            Assert.AreEqual("Jane Doe", matchById.Name);
            /************/
        }
    }
}
