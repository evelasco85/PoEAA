using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Tests.CustomerServices;
using static Framework.Tests.CustomerServices.GetCustomersByCivilStatusQuery.Criteria;

namespace Framework.Tests
{
    [TestClass]
    public class QueryObjectTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void TestGetCustomerByIdQuery()
        {
            GetCustomerByIdQuery query = new GetCustomerByIdQuery
            {
                CriteriaInput = GetCustomerByIdQuery.Criteria.SearchById(2)
            };

            Customer matchById = query.ConcreteExecute();

            Assert.AreEqual("2", matchById.Number);
            Assert.AreEqual("Jane Doe", matchById.Name);
        }

        [TestMethod]
        public void TestGetCustomerByCivilStatusQuery()
        {
            GetCustomersByCivilStatusQuery query = new GetCustomersByCivilStatusQuery
            {
                CriteriaInput = GetCustomersByCivilStatusQuery.Criteria.SearchByStatus(CivilStatus.Married)
            };

            IList<Customer> matchedCustomers = query.ConcreteExecute();

            Assert.AreEqual(2, matchedCustomers.Count);
            Assert.AreEqual("5", matchedCustomers[0].Number);
            Assert.AreEqual("Test Married", matchedCustomers[0].Name);
            Assert.AreEqual("7", matchedCustomers[1].Number);
            Assert.AreEqual("Test Married", matchedCustomers[1].Name);
        }
    }
}
