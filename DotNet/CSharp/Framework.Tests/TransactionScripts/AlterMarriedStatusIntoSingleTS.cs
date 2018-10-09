using System.Collections.Generic;
using Framework.Tests.CustomerServices;

namespace Framework.Tests.TransactionScripts
{
    public class AlterMarriedStatusIntoSingleTS : TransactionScript<GetCustomersByCivilStatusQuery.Criteria, AlterMarriedStatusIntoSingleTS.TransactionResult>
    {
        public class TransactionResult
        {
            public IList<Customer> SuccessfullyAlteredCustomers { get; set; }
        }

        public AlterMarriedStatusIntoSingleTS(IDomainObjectManager manager) :
            base(
            manager,
            manager
            )
        {
        }

        public override TransactionResult ExecutionBody()
        {
            IRepository<Customer> repository = RepositoryRegistry.GetRepository<Customer>();
            GetCustomersByCivilStatusQuery.Criteria criteriaByStatus = Input;
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
