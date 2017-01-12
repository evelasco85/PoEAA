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
                SearchInput = 2
            };

            IList<Customer> customers = query.Execute();
            Customer result = customers.First();

            Assert.AreEqual("2", result.Number);
            Assert.AreEqual("Jane Doe", result.Name);
        }
    }
}
