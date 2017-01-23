using System;
using System.Text;
using System.Collections.Generic;
using Framework.Tests.CustomerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    /// <summary>
    /// Summary description for IdentityMapTests
    /// </summary>
    [TestClass]
    public class IdentityMapTests
    {
        [TestMethod]
        public void Test_DuplicateEntry()
        {
            Customer customer1 = new Customer(null) {Number = "001"};
            IIdentityMap<Customer> map = new IdentityMap<Customer>();

            map.AddEntity(customer1);
            map.AddEntity(customer1);

            Assert.AreEqual(1, map.Count);
        }

        [TestMethod]
        public void Test_Identity_Updates()
        {
            Customer customer1 = new Customer(null) { Number = "001" };
            IIdentityMap<Customer> map = new IdentityMap<Customer>();

            map.AddEntity(customer1);

            customer1.Number = "002";

            map.AddEntity(customer1);

            Assert.AreEqual(1, map.Count);
        }

        [TestMethod]
        public void Test_Retrieval_By_Keys()
        {
            Customer customer1 = new Customer(null) { Number = "001" };
            Customer customer2 = new Customer(null) { Number = "002" };
            Customer customer3 = new Customer(null) { Number = "003" };
            Customer customer4 = new Customer(null) { Number = "004" };
            Customer customer5 = new Customer(null) { Number = "005" };
            IIdentityMap<Customer> map = new IdentityMap<Customer>();

            map.AddEntity(customer1);
            map.AddEntity(customer2);
            map.AddEntity(customer3);
            map.AddEntity(customer4);
            map.AddEntity(customer5);

            Assert.AreEqual(5, map.Count);

            map.SearchBy()
                .SetFilter(customer => customer.Number, "001")
                .GetEntity();
        }
    }
}
