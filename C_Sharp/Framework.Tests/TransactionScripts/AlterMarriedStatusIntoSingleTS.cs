using System.Collections.Generic;
using Framework.Tests.CustomerServices;

namespace Framework.Tests.TransactionScripts
{
    public class AlterMarriedStatusIntoSingleTS : TransactionScript<GetCustomerByCivilStatusQuery.Criteria, AlterMarriedStatusIntoSingleTS.TransactionResult>
    {
        public class TransactionResult
        {
            public IList<Customer> SuccessfullyAlteredCustomers { get; set; }
        }

        public AlterMarriedStatusIntoSingleTS() :
            base(
            DataSynchronizationManager.GetInstance(),
            DataSynchronizationManager.GetInstance()
            )
        {
        }

        public override TransactionResult ExecutionBody()
        {
            IRepository<Customer> repository = RepositoryRegistry.GetRepository<Customer>();
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = Input;
            IUnitOfWork uow = CreateUnitOfWork();

            ((List<Customer>) repository.Matching(criteriaByStatus)).ForEach(customer =>
            {
                customer.Name = "Test is now single";

                uow.RegisterDirty(customer);
            });

            TransactionResult result = new TransactionResult
            {
                SuccessfullyAlteredCustomers = new List<Customer>()
            };

            uow.Commit(
                (domainObject, action, info) =>
                {
                    result.SuccessfullyAlteredCustomers.Add((Customer) domainObject);
                },
                null);

            return result;
        }
    }
}
