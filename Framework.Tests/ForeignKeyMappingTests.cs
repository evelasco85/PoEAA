using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Framework.Data_Manipulation;
using Framework.Tests.CustomerServices;
using Framework.Tests.Mapping;
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
        private IDataSynchronizationManager _dataSyncManager;
        private IForeignKeyMappingManager _fkMappingManager;

        [TestInitialize]
        public void Initialize()
        {
            _dataSyncManager = DataSynchronizationManager.GetInstance();
            _fkMappingManager = ForeignKeyMappingManager.GetInstance();

            _dataSyncManager.RegisterEntity(
                new CustomerMapper(), 
                new List<IBaseQueryObject<Customer>> {
                    {new GetCustomerByIdQuery()},
                    {new GetCustomerByCivilStatusQuery()}
                });

            _dataSyncManager.RegisterEntity(
                new AccountReceivableMapper(),
                new List<IBaseQueryObject<AccountReceivable>> {
                    {new GetAccountReceivablesByCustomerId()}
                });

            _fkMappingManager.RegisterForeignKeyMapping(new FK_Customer_AccountReceivable());
        }

        [TestMethod]
        public void TestForeignKeyMapping()
        {
            IRepository<Customer> repository = _dataSyncManager.GetRepository<Customer>();

            /*Match by Id*/
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(3);
            IList<Customer> resultsById = repository.Matching(criteriaById);
            Customer matchById = resultsById.First();

            Assert.AreEqual("3", matchById.Number);
            Assert.AreEqual("John Doe", matchById.Name);
            /************/

            IList<AccountReceivable> receivables = _fkMappingManager.GetForeignKeyValues<Customer, AccountReceivable>(matchById);

            Assert.AreEqual(2, receivables.Count);
            Assert.AreEqual("02", receivables[0].Number);
            Assert.AreEqual("04", receivables[1].Number);
        }
    }
}
