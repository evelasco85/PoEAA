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
        public void TestCommit_SimpleInsertion()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = GetCustomerByCivilStatusQuery.Criteria.SearchByStatus(GetCustomerByCivilStatusQuery.CivilStatus.Married);
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            List<Customer> customerResults = new List<Customer>();
            IUnitOfWork uow = new UnitOfWork();

            customerResults.AddRange(repository.Matching(criteriaByStatus));
            customerResults.AddRange(repository.Matching(criteriaById));

            
            IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper<Customer>();
            Customer customer1 = new Customer(mapper) { Number = "1" };
            Customer customer2 = new Customer(mapper) { Number = "2" };
            Customer customer3 = new Customer(mapper) { Number = "3" };


            //Sequence of observation affects commit order
            uow.RegisterNew(customer1);
            uow.RegisterNew(customer3);
            uow.RegisterNew(customer2);
            
            IList<string> sequenceDescription = new List<string>();

            uow.Commit(
                (domainObject, action, results) =>
                {
                    string description = BaseMapper.GetHashValue<string>(results, CustomerMapper.SUCCESS_DESCRIPTION);

                    sequenceDescription.Add(string.Format("{0}={1}={2}", description, action.ToString(), domainObject.Mapper.GetEntityTypeName()));
                },
                (domainObject, action, results) => { }
                );

            Assert.AreEqual("1=Insert=Customer", sequenceDescription[0]);
            Assert.AreEqual("3=Insert=Customer", sequenceDescription[1]);
            Assert.AreEqual("2=Insert=Customer", sequenceDescription[2]);
        }

        [TestMethod]
        public void TestCommit_RemoveInsertRegistration()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = GetCustomerByCivilStatusQuery.Criteria.SearchByStatus(GetCustomerByCivilStatusQuery.CivilStatus.Married);
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            List<Customer> customerResults = new List<Customer>();
            IUnitOfWork uow = new UnitOfWork();

            customerResults.AddRange(repository.Matching(criteriaByStatus));
            customerResults.AddRange(repository.Matching(criteriaById));


            IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper<Customer>();
            Customer customer1 = new Customer(mapper) { Number = "1" };
            Customer customer2 = new Customer(mapper) { Number = "2" };
            Customer customer3 = new Customer(mapper) { Number = "3" };


            //Sequence of observation affects commit order
            uow.RegisterNew(customer1);
            uow.RegisterNew(customer3);
            uow.RegisterNew(customer2);

            uow.RemoveExistingRegistration(customer1);
            IList<string> sequenceDescription = new List<string>();

            uow.Commit(
                (domainObject, action, results) =>
                {
                    string description = BaseMapper.GetHashValue<string>(results, CustomerMapper.SUCCESS_DESCRIPTION);

                    sequenceDescription.Add(string.Format("{0}={1}={2}", description, action.ToString(), domainObject.Mapper.GetEntityTypeName()));
                },
                (domainObject, action, results) => { }
                );

            Assert.AreEqual("3=Insert=Customer", sequenceDescription[0]);
            Assert.AreEqual("2=Insert=Customer", sequenceDescription[1]);
        }
    }
}
