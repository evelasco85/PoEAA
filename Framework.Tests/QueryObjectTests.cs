using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Tests.CustomerServices;

namespace Framework.Tests
{
    [TestClass]
    public class QueryObjectTests
    {
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
