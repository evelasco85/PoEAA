using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Framework.Data_Manipulation;
using Framework.Tests.CustomerServices;
using Framework.Tests.ReceivableServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    /// <summary>
    /// Summary description for RepositoryTests
    /// </summary>
    [TestClass]
    public class ForeignKeyMappingTests
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

            _manager.RegisterEntity(
                new AccountReceivableMapper(),
                new List<IBaseQueryObject<AccountReceivable>> {
                    {new GetAccountReceivablesByCustomerId()}
                });
        }

        [TestMethod]
        public void TestForeignKeyMapping()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();

            /*Match by Id*/
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(3);
            IList<Customer> resultsById = repository.Matching(criteriaById);
            Customer matchById = resultsById.First();

            Assert.AreEqual("3", matchById.Number);
            Assert.AreEqual("John Doe", matchById.Name);
            /************/

            IList<AccountReceivable> receivables = matchById.AccountReceivables;

            Assert.AreEqual(2, receivables.Count);
            Assert.AreEqual("02", receivables[0].Number);
            Assert.AreEqual("04", receivables[1].Number);
        }
    }
}
