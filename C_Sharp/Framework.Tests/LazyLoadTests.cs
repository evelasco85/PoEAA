using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.LazyLoad;
using Framework.Tests.CustomerServices;
using Framework.Tests.LazyLoad;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    [TestClass]
    public class LazyLoadTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void TestLazyLoadProducts()
        {
            LazyLoader<ProductDomain, ProductDomain.Criteria> loader = new ProductLazyLoader(null);
            LazyLoadList<ProductDomain, ProductDomain.Criteria> list = new LazyLoadList<ProductDomain, ProductDomain.Criteria>(loader);

            list.Add(new ProductDomain(null, ProductDomain.Criteria.SearchById(2)));
            list.Add(new ProductDomain(null, ProductDomain.Criteria.SearchById(4)));
            list.Add(new ProductDomain(null, ProductDomain.Criteria.SearchById(5)));

            Assert.AreEqual("Product two", list[0].Description);
            Assert.AreEqual("Product four", list[1].Description);
            Assert.AreEqual("Product five", list[2].Description);
        }
    }
}
