using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Tests.CustomerServices;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.Tests
{
    [TestClass]
    public class DomainObjectTests
    {
        [TestMethod]
        public void TestGetMonitoredProperties()
        {
            Customer customer = new Customer(null);

            IDictionary<string, PropertyInfo> monitoredProperties = customer.GetMonitoredProperties();

            Assert.AreEqual(2, monitoredProperties.Count);
            Assert.IsTrue(monitoredProperties.ContainsKey("Number"));
            Assert.IsTrue(monitoredProperties.ContainsKey("Name"));
        }
    }
}
