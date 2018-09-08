using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Tests.CustomerServices;
using Framework.Domain;
using Framework.Tests.ReceivableServices;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.Tests
{
    [TestClass]
    public class ObjectStateMonitoringContainerTests
    {
        [TestMethod]
        public void TestGetModifiedEntities()
        {
            string originalName = "Christine Dela Cruz";
            Customer customer1 = new Customer(null, null) { Number = "1", Name = "Juan Dela Cruz" };
            Customer customer2 = new Customer(null, null) { Number = "2", Name = originalName };
            Customer customer3 = new Customer(null, null) { Number = "3", Name = "Jane Doe" };

            string originalNumber = "01";
            string originalCustomerNumber = "001";
            AccountReceivable receivable1 = new AccountReceivable(null) { Number = originalNumber, CustomerNumber = originalCustomerNumber };
            AccountReceivable receivable2 = new AccountReceivable(null) { Number = "02", CustomerNumber = "010" };
            AccountReceivable receivable3 = new AccountReceivable(null) { Number = "03", CustomerNumber = "100" };

            ObjectStateMonitoringContainer stateContainer = new ObjectStateMonitoringContainer();

            stateContainer.Monitor(customer1);
            stateContainer.Monitor(customer1);
            stateContainer.Monitor(customer2);
            stateContainer.Monitor(customer3);
            stateContainer.Monitor(receivable1);
            stateContainer.Monitor(receivable2);
            stateContainer.Monitor(receivable3);

            customer2.Name = "John Doe";
            receivable1.Number = "023";
            receivable1.CustomerNumber = "200";

            IList<ObjectStateMonitoringContainer.ModifiedEntity> modifiedItems = stateContainer.GetModifiedEntities();

            Assert.IsTrue(modifiedItems[0].DomainObject.SystemId == customer2.SystemId);
            Assert.AreEqual(1, modifiedItems[0].DifferingProperties.Count);

            PropertyInfo propertyInfo = modifiedItems[0].DifferingProperties[0];
            Assert.AreEqual("Name", propertyInfo.Name);
            Assert.AreEqual(originalName, modifiedItems[0].OriginalValues[propertyInfo.Name]);
            Assert.AreNotEqual(
                modifiedItems[0].OriginalValues[propertyInfo.Name],
                modifiedItems[0].NewValues[propertyInfo.Name]);

            //-------------------------
            Assert.IsTrue(modifiedItems[1].DomainObject.SystemId == receivable1.SystemId);
            Assert.AreEqual(2, modifiedItems[1].DifferingProperties.Count);

            PropertyInfo propertyInfo_01 = modifiedItems[1].DifferingProperties[0];
            Assert.AreEqual("Number", propertyInfo_01.Name);
            Assert.AreEqual(originalNumber, modifiedItems[1].OriginalValues[propertyInfo_01.Name]);
            Assert.AreNotEqual(
                modifiedItems[1].OriginalValues[propertyInfo_01.Name],
                modifiedItems[1].NewValues[propertyInfo_01.Name]);

            PropertyInfo propertyInfo_02 = modifiedItems[1].DifferingProperties[1];
            Assert.AreEqual("CustomerNumber", propertyInfo_02.Name);
            Assert.AreEqual(originalCustomerNumber, modifiedItems[1].OriginalValues[propertyInfo_02.Name]);
            Assert.AreNotEqual(
                modifiedItems[1].OriginalValues[propertyInfo_02.Name],
                modifiedItems[1].NewValues[propertyInfo_02.Name]);
        }
    }
}
