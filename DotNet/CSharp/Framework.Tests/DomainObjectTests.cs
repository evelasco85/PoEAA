using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Tests.CustomerServices;
using System.Collections.Generic;
using System.Reflection;
using System;
using Framework.Tests.ReceivableServices;

namespace Framework.Tests
{
    [TestClass]
    public class DomainObjectTests
    {
        [TestMethod]
        public void TestGetMonitoredProperties()
        {
            Customer customer = new Customer(null, null);

            IDictionary<string, PropertyInfo> monitoredProperties = customer.GetMonitoredProperties();

            Assert.AreEqual(2, monitoredProperties.Count);
            Assert.IsTrue(monitoredProperties.ContainsKey("Number"));
            Assert.IsTrue(monitoredProperties.ContainsKey("Name"));
        }

        [TestMethod]
        public void TestGetCurrentMonitoredPropertyValues()
        {
            Customer customer = new Customer(null, null)
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

        [TestMethod]
        public void TestGetDiffProperties_NotEquals()
        {
            Customer customer = new Customer(null, null)
            {
                Name = "Juan Dela Cruz",
                Number = "010",
                NotSoHelpfulMember = true,
                Temp = new Customer.InnerClass()
            };

            Customer customer2 = new Customer(null, null)
            {
                Name = "John Doe",
                Number = null,
                NotSoHelpfulMember = true,
                Temp = new Customer.InnerClass()
            };

            IDictionary<string, PropertyInfo> diffProperties = customer.GetDiffProperties(customer2);

            Assert.AreEqual(2, diffProperties.Count);
            Assert.IsTrue(diffProperties.ContainsKey("Name"));
            Assert.IsTrue(diffProperties.ContainsKey("Number"));
            Assert.AreNotEqual(diffProperties["Name"].GetValue(customer), diffProperties["Name"].GetValue(customer2));
            Assert.AreNotEqual(diffProperties["Number"].GetValue(customer), diffProperties["Number"].GetValue(customer2));
        }

        [TestMethod]
        public void TestGetDiffProperties_Equals()
        {
            Customer customer = new Customer(null, null)
            {
                Name = "Juan Dela Cruz",
                Number = "010",
                NotSoHelpfulMember = true,
                Temp = new Customer.InnerClass()
            };

            Customer customer2 = new Customer(null, null)
            {
                Name = "Juan Dela Cruz",
                Number = "010",
                NotSoHelpfulMember = true,
                Temp = new Customer.InnerClass()
            };

            IDictionary<string, PropertyInfo> diffProperties = customer.GetDiffProperties(customer2);

            Assert.AreEqual(0, diffProperties.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetDiffProperties_NotEquals_NullArg()
        {
            Customer customer = new Customer(null, null)
            {
                Name = "Juan Dela Cruz",
                Number = "010",
                NotSoHelpfulMember = true,
                Temp = new Customer.InnerClass()
            };

            IDictionary<string, PropertyInfo> diffProperties = customer.GetDiffProperties(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetDiffProperties_NotEquals_TypeMismatch()
        {
            Customer customer = new Customer(null, null)
            {
                Name = "Juan Dela Cruz",
                Number = "010",
                NotSoHelpfulMember = true,
                Temp = new Customer.InnerClass()
            };

            AccountReceivable receivable = new AccountReceivable(null);

            IDictionary<string, PropertyInfo> diffProperties = customer.GetDiffProperties(receivable);
        }
    }
}
