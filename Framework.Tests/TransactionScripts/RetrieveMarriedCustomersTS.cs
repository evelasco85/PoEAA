using System.Collections.Generic;
using Framework.Tests.CustomerServices;

namespace Framework.Tests.TransactionScripts
{
    public class RetrieveMarriedCustomersTS : TransactionScript<GetCustomerByCivilStatusQuery.Criteria, IList<Customer>>
    {
        public RetrieveMarriedCustomersTS() :
            base(
            DataSynchronizationManager.GetInstance(),
            DataSynchronizationManager.GetInstance()
            )
        {
        }

        public override IList<Customer> ExecutionBody()
        {
            IRepository<Customer> repository = RepositoryRegistry.GetRepository<Customer>();

            /*Match by civil status*/
            GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = Input;
            IList<Customer> resultsByStatus = repository.Matching(criteriaByStatus);

            return resultsByStatus;
        }
    }
}
