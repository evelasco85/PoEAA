using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Tests.Entities;
using Framework.Tests.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Tests.Queries;

namespace Framework.Tests
{
    [TestClass]
    public class CustomerTests
    {
        IDataSynchronizationManager manager = DataSynchronizationManager.GetInstance();

        [TestInitialize]
        public void Initialize()
        {
            manager.RegisterEntity(new CustomerMapper());
        }

        [TestMethod]
        public void TestGetCustomerByIdCriteria()
        {
            GetCustomerByIdQuery criteria = new GetCustomerByIdQuery
            {
                SearchInput = 2
            };

            IList<Customer> customers = criteria.Execute();
            Customer result = customers.First();

            Assert.AreEqual("2", result.Number);
            Assert.AreEqual("Jane Doe", result.Name);
        }
    }
}
