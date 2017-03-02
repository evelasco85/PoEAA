using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.LazyLoad;
using Framework.Tests.CustomerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    [TestClass]
    public class LazyLoadTests
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
        public void TestLazyLoadCustomer()
        {
            //LazyLoadManager.GetInstance().RegisterLazyLoadType<Customer, GetCustomerByIdQuery.Criteria>();
            //GetCustomerByIdQuery.Criteria criteria = GetCustomerByIdQuery.Criteria.SearchById(23);
            //Customer lazyLoadCustomer = LazyLoadManager.GetInstance().CreateLazyLoadEntity<Customer, GetCustomerByIdQuery.Criteria>(criteria);
        }
    }
}
