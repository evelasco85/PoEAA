using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Tests.CustomerServices;
using Framework.Data_Manipulation;

namespace Framework.Tests
{
    [TestClass]
    public class QueryObjectTests
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
        public void TestGetCustomerByIdQuery()
        {
            GetCustomerByIdQuery query = new GetCustomerByIdQuery
            {
                SearchInput = GetCustomerByIdQuery.Criteria.SearchById(2)
            };

            IList<Customer> resultsById = query.Execute();
            Customer matchById = resultsById.First();

            Assert.AreEqual("2", matchById.Number);
            Assert.AreEqual("Jane Doe", matchById.Name);
        }
    }
}
