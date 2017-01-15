using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;
using Framework.Domain;
using Framework.Tests.CustomerServices;
using  Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        private IDataSynchronizationManager _manager;
        private CustomerMapper _mapper = new CustomerMapper();

        [TestInitialize]
        public void Initialize()
        {
            _manager = DataSynchronizationManager.GetInstance();

            _manager.RegisterEntity(
                _mapper,
                new List<IBaseQueryObject<Customer>> {
                    {new GetCustomerByIdQuery()},
                    {new GetCustomerByCivilStatusQuery()}
                });
        }

        [TestMethod]
        public void TestGetUncommitedCount()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = GetCustomerByCivilStatusQuery.Criteria.SearchById(GetCustomerByCivilStatusQuery.CivilStatus.Married);
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            List<Customer> results = new List<Customer>();
            IUnitOfWork uow = new UnitOfWork();
            
            results.AddRange(repository.Matching(criteriaByStatus));
            results.AddRange(repository.Matching(criteriaById));

            /*Entities loaded from repository will be marked as 'Clean'*/
            results.ForEach(customer =>
            {
                uow.ObserveEntityForChanges(customer);
                Assert.AreEqual(DomainObjectState.Clean, customer.GetCurrentState(), "Entities loaded from data source or repository should be marked as 'Clean'");
            });

            IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper<Customer>();
            Customer customer1 = new Customer(mapper);
            Customer customer2 = new Customer(mapper);
            Customer customer3 = new Customer(mapper);

            uow.ObserveEntityForChanges(customer1);
            uow.ObserveEntityForChanges(customer2);
            uow.ObserveEntityForChanges(customer3);

            Assert.AreEqual(3, uow.GetUncommitedCount());
        }

        [TestMethod]
        public void TestCommit_SimpleInsertion()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = GetCustomerByCivilStatusQuery.Criteria.SearchById(GetCustomerByCivilStatusQuery.CivilStatus.Married);
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            List<Customer> results = new List<Customer>();
            IUnitOfWork uow = new UnitOfWork();

            results.AddRange(repository.Matching(criteriaByStatus));
            results.AddRange(repository.Matching(criteriaById));

            /*Entities loaded from repository will be marked as 'Clean'*/
            results.ForEach(customer =>
            {
                uow.ObserveEntityForChanges(customer);
                Assert.AreEqual(DomainObjectState.Clean, customer.GetCurrentState(), "Entities loaded from data source or repository should be marked as 'Clean'");
            });

            //Observe sequence of instantiation
            IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper<Customer>();
            Customer customer1 = new Customer(mapper) { Number = "1" };
            Customer customer3 = new Customer(mapper) { Number = "3" };
            Customer customer2 = new Customer(mapper) { Number = "2" };

            uow.ObserveEntityForChanges(customer1);
            uow.ObserveEntityForChanges(customer2);
            uow.ObserveEntityForChanges(customer3);

            Assert.AreEqual(3, uow.GetUncommitedCount());

            IList<string> sequenceDescription = new List<string>();

            uow.Commit(
                (domainObject, action, additionalInfo) =>
                {
                    sequenceDescription.Add(string.Format("{0}={1}={2}", (string)additionalInfo, action.ToString(), domainObject.Mapper.GetEntityTypeName()));
                },
                (domainObject, action, exception, additionalInfo) => { }
                );

            Assert.AreEqual("1=Insert=Customer", sequenceDescription[0]);
            Assert.AreEqual("3=Insert=Customer", sequenceDescription[1]);
            Assert.AreEqual("2=Insert=Customer", sequenceDescription[2]);
        }
    }
}
