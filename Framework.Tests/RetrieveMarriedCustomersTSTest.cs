using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Tests.CustomerServices;
using Framework.Tests.TransactionScripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    [TestClass]
    public class RetrieveMarriedCustomersTSTest
    {
        private IDataSynchronizationManager _manager;

        [TestInitialize]
        public void Initialize()
        {
            _manager = DataSynchronizationManager.GetInstance();

            _manager.RegisterEntity(
                new CustomerMapper(),
                new List<IBaseQueryObject<Customer>> {
                    {new GetCustomerByIdQuery()},
                    {new GetCustomerByCivilStatusQuery()}
                });
        }

        [TestMethod]
        public void TestRunScript()
        {
            RetrieveMarriedCustomersTS transactionScript = new RetrieveMarriedCustomersTS();

            transactionScript.Input = GetCustomerByCivilStatusQuery.Criteria.SearchByStatus(GetCustomerByCivilStatusQuery.CivilStatus.Married);

            transactionScript.RunScript();

            IList<Customer> resultsByStatus = transactionScript.Output;

            Assert.AreEqual(1, resultsByStatus.Count);
            Assert.AreEqual("5", resultsByStatus[0].Number);
            Assert.AreEqual("Test Married", resultsByStatus[0].Name);
        }
    }
}
