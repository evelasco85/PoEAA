﻿using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Tests.CustomerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Framework.Tests.CustomerServices.GetCustomersByCivilStatusQuery.Criteria;

namespace Framework.Tests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        private IDomainObjectManager _manager = new DomainObjectManager();
        private CustomerMapper _mapper = new CustomerMapper();

        [TestInitialize]
        public void Initialize()
        {
            _manager.RegisterEntity(
                _mapper,
                new List<IBaseQueryObject<Customer>> {
                    {new GetCustomerByIdQuery()},
                    {new GetCustomersByCivilStatusQuery()}
                });
        }

        [TestMethod]
        public void TestCommit_SimpleInsertion()
        {
            IRepository<Customer> repository = _manager.GetRepository<Customer>();
            GetCustomersByCivilStatusQuery.Criteria criteriaByStatus = GetCustomersByCivilStatusQuery.Criteria.SearchByStatus(CivilStatus.Married);
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            List<Customer> customerResults = new List<Customer>();
            IUnitOfWork uow = new UnitOfWork();

            customerResults.AddRange(repository.Matching(criteriaByStatus));
            customerResults.Add(repository.Matching(criteriaById));

            
            IBaseMapper mapper = _manager.GetMapper<Customer>();
            Customer customer1 = new Customer(mapper, null) { Number = "1" };
            Customer customer2 = new Customer(mapper, null) { Number = "2" };
            Customer customer3 = new Customer(mapper, null) { Number = "3" };


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
            GetCustomersByCivilStatusQuery.Criteria criteriaByStatus = GetCustomersByCivilStatusQuery.Criteria.SearchByStatus(CivilStatus.Married);
            GetCustomerByIdQuery.Criteria criteriaById = GetCustomerByIdQuery.Criteria.SearchById(2);
            List<Customer> customerResults = new List<Customer>();
            IUnitOfWork uow = new UnitOfWork();

            customerResults.AddRange(repository.Matching(criteriaByStatus));
            customerResults.Add(repository.Matching(criteriaById));


            IBaseMapper mapper = _manager.GetMapper<Customer>();
            Customer customer1 = new Customer(mapper, null) { Number = "1" };
            Customer customer2 = new Customer(mapper, null) { Number = "2" };
            Customer customer3 = new Customer(mapper, null) { Number = "3" };


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
