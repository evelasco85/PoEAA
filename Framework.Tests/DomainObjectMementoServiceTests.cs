using System;
using Framework.Data_Manipulation;
using Framework.Domain;
using Framework.Tests.CustomerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests
{
    [TestClass]
    public class DomainObjectMementoServiceTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            IDomainObjectMementoService service = DomainObjectMementoService.GetInstance();
            Customer customer = new Customer(null) {Name = "John Doe", Number = "123"};
            IDomainObjectMemento memento = service.CreateMemento(customer);

            Assert.AreEqual("John Doe", memento.GetPropertyValue("Name"));
            Assert.AreEqual("123", memento.GetPropertyValue("Number"));
        }
    }
}
