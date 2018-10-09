using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Tests.CustomerServices;
using Framework.Tests.TransactionScripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Framework.Tests.CustomerServices.GetCustomersByCivilStatusQuery.Criteria;

namespace Framework.Tests
{
    [TestClass]
    public class TransactionScriptTest
    {
        private IDomainObjectManager _manager = new DomainObjectManager();

        [TestInitialize]
        public void Initialize()
        {
            _manager.RegisterEntity(
                new CustomerMapper(),
                new List<IBaseQueryObject<Customer>> {
                    {new GetCustomerByIdQuery()},
                    {new GetCustomersByCivilStatusQuery()}
                });
        }

        [TestMethod]
        public void TestRunScript()
        {
            AlterMarriedStatusIntoSingleTS transactionScript = new AlterMarriedStatusIntoSingleTS(_manager);

            transactionScript.Input = GetCustomersByCivilStatusQuery.Criteria.SearchByStatus(CivilStatus.Married);

            transactionScript.RunScript();

            IList<Customer> resultsByStatus = transactionScript.Output.SuccessfullyAlteredCustomers;

            Assert.AreEqual(2, resultsByStatus.Count);
            Assert.AreEqual("5", resultsByStatus[0].Number);
            Assert.AreEqual("Test is now single", resultsByStatus[0].Name);
            Assert.AreEqual("7", resultsByStatus[1].Number);
            Assert.AreEqual("Test is now single", resultsByStatus[1].Name);

        }
    }
}
