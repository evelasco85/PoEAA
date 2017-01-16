using Framework.Tests.CustomerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    [TestClass]
    public class UnitOfWorkEntityWrapperTests
    {
        [TestMethod]
        public void TestHasChanges()
        {
            Customer customer = new Customer(null) {Name = "John Doe", Number = "123"};
            IUnitOfWorkEntityWrapper wrapper = new UnitOfWorkEntityWrapper<Customer>(customer, UnitOfWorkAction.Insert);

            Assert.IsTrue(!string.IsNullOrEmpty(wrapper.OriginalHashCode));
            Assert.IsFalse(wrapper.HasChanges());

            customer.Name = "Juan Dela Cruz";
            customer.Number = "345";

            Assert.IsTrue(wrapper.HasChanges());
        }
    }
}
