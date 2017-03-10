using System.Collections.Generic;
using System.Linq;
using Framework.Data_Manipulation;
using Framework.LazyLoad;
using Framework.Tests.LazyLoad;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    [TestClass]
    public class LazyLoadTests
    {

        /****************************/
        //
        //      PLEASE READ 'LazyLoad_README.txt'
        //
        /****************************/
        [TestMethod]
        public void TestLazyLoadProducts()
        {
            LazyLoader<ProductDomain, ProductDomain.Criteria> loader = new ProductLazyLoader();
            IList<ProductDomain> list = new LazyLoadList<ProductDomain, ProductDomain.Criteria>(loader);
            IBaseMapper mapper = null;

            list.Add(new ProductDomain(mapper, ProductDomain.Criteria.SearchById(2)));
            list.Add(new ProductDomain(mapper, ProductDomain.Criteria.SearchById(4)));
            list.Add(new ProductDomain(mapper, ProductDomain.Criteria.SearchById(5)));

            ProductDomain product = list.FirstOrDefault(x => x.Id == 2);

            Assert.IsTrue(string.IsNullOrEmpty(product.Description));       //Description is not loaded
            Assert.AreEqual("Product two", list[0].Description);
            Assert.AreEqual("Product four", list[1].Description);
            Assert.AreEqual("Product five", list[2].Description);
            Assert.IsFalse(string.IsNullOrEmpty(product.Description));      //Description is now loaded
        }
    }
}
