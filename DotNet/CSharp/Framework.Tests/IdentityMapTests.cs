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
            Customer customer1 = new Customer(null, null) {Number = "001"};
            IIdentityMap<Customer> map = new IdentityMap<Customer>();

            map.AddEntity(customer1);
            map.AddEntity(customer1);

            Assert.AreEqual(1, map.Count);
        }

        [TestMethod]
        public void Test_Identity_Updates()
        {
            Customer customer1 = new Customer(null, null) { Number = "001" , Name = "John Doe"};
            IIdentityMap<Customer> map = new IdentityMap<Customer>();

            map.AddEntity(customer1);

            customer1.Number = "002";

            map.AddEntity(customer1);       //Updating primary key values

            Customer matchedCustomer = map.SearchBy().SetFilter(customer => customer.Number, "002").GetEntity();
            Customer notFoundCustomer = map.SearchBy().SetFilter(customer => customer.Number, "001").GetEntity();

            Assert.AreEqual(1, map.Count);
            Assert.IsTrue(notFoundCustomer == null);
            Assert.IsNotNull(matchedCustomer);
            Assert.AreEqual(customer1.Number, matchedCustomer.Number);
            Assert.AreEqual(customer1.Name, matchedCustomer.Name);
        }

        [TestMethod]
        public void Test_Retrieval_By_Keys()
        {
            Customer customer1 = new Customer(null, null) { Number = "001" };
            Customer customer2 = new Customer(null, null) { Number = "002" };
            Customer customer3 = new Customer(null, null) { Number = "003" };
            Customer customer4 = new Customer(null, null) {Number = "004", Name = "John Doe"};
            Customer customer5 = new Customer(null, null) { Number = "005" };
            IIdentityMap<Customer> map = new IdentityMap<Customer>();

            map.AddEntity(customer1);
            map.AddEntity(customer2);
            map.AddEntity(customer3);
            map.AddEntity(customer4);
            map.AddEntity(customer5);

            Assert.AreEqual(5, map.Count);

            Customer matchedCustomer =  map.SearchBy()
                .SetFilter(customer => customer.Number, "004")
                .GetEntity();

            Assert.AreEqual(customer4.Number, matchedCustomer.Number);
            Assert.AreEqual(customer4.Name, matchedCustomer.Name);
        }

        [TestMethod]
        public void Test_SearchResult_NotFound()
        {
            Customer customer1 = new Customer(null, null) {Number = "001"};
            Customer customer2 = new Customer(null, null) {Number = "002"};
            Customer customer3 = new Customer(null, null) {Number = "003"};
            Customer customer4 = new Customer(null, null) {Number = "004", Name = "John Doe"};
            Customer customer5 = new Customer(null, null) {Number = "005"};
            IIdentityMap<Customer> map = new IdentityMap<Customer>();

            map.AddEntity(customer1);
            map.AddEntity(customer2);
            map.AddEntity(customer3);
            map.AddEntity(customer4);
            map.AddEntity(customer5);

            Assert.AreEqual(5, map.Count);

            Customer matchedCustomer = map.SearchBy()
                .SetFilter(customer => customer.Number, "007")
                .GetEntity();

            Assert.IsNull(matchedCustomer);
        }

        [TestMethod]
        public void Test_Clear_Identities()
        {
            Customer customer1 = new Customer(null, null) { Number = "001" };
            Customer customer2 = new Customer(null, null) { Number = "002" };
            Customer customer3 = new Customer(null, null) { Number = "003" };
            Customer customer4 = new Customer(null, null) { Number = "004", Name = "John Doe" };
            Customer customer5 = new Customer(null, null) { Number = "005" };
            IIdentityMap<Customer> map = new IdentityMap<Customer>();

            map.AddEntity(customer1);
            map.AddEntity(customer2);
            map.AddEntity(customer3);
            map.AddEntity(customer4);
            map.AddEntity(customer5);

            Assert.AreEqual(5, map.Count);

            map.ClearEntities();

            Assert.AreEqual(0, map.Count);
        }
    }
}
