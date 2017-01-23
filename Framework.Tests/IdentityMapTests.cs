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
    }
}
