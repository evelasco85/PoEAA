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
        private IDomainObjectManager _dataSyncManager = new DomainObjectManager();
        private IForeignKeyMappingManager _fkMappingManager;

        [TestInitialize]
        public void Initialize()
        {
            _fkMappingManager = ForeignKeyMappingManager.GetInstance();

            _dataSyncManager.RegisterEntity(
                new CustomerMapper(), 
                new List<IBaseQueryObject<Customer>> {
                    {new GetCustomerByIdQuery()},
                    {new GetCustomersByCivilStatusQuery()}
                });

            _dataSyncManager.RegisterEntity(
                new AccountReceivableMapper(),
                new List<IBaseQueryObject<AccountReceivable>> {
                    {new GetAccountReceivablesByCustomerId()}
                });

            _fkMappingManager.RegisterForeignKeyMapping(new FK_Customer_AccountReceivable(_dataSyncManager));
        }

        [TestMethod]
        public void TestForeignKeyMapping()
        {
            IRepository<Customer> repository = _dataSyncManager.GetRepository<Customer>();

            /*Match by Id*/
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(3);
            Customer matchById = repository.Matching(criteriaById);

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
