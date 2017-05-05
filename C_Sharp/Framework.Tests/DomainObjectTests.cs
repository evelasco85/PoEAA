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

        [TestMethod]
        public void TestGetCurrentMonitoredPropertyValues()
        {
            Customer customer = new Customer(null)
            {
                Name = "Juan Dela Cruz",
                Number = "010",
                NotSoHelpfulMember = true,
                Temp = new Customer.InnerClass()
            };

            IDictionary<string, object> monitoredValues = customer.GetCurrentMonitoredPropertyValues();

            Assert.AreEqual(2, monitoredValues.Count);
            Assert.IsTrue(string.Equals(monitoredValues["Number"], "010"));
            Assert.IsTrue(string.Equals(monitoredValues["Name"], "Juan Dela Cruz"));
        }
    }
}
