package TransactionScripts;

import com.DataSynchronizationManager;
import com.Interfaces.IRepository;
import com.Interfaces.IUnitOfWork;
import com.TransactionScript;

import java.rmi.NoSuchObjectException;
import java.util.ArrayList;
import java.util.List;

import CustomerServices.Customer;
import CustomerServices.CustomerInvocationDelegates;
import CustomerServices.GetCustomerByCivilStatusQuery;

/**
 * Created by aiko on 4/8/17.
 */

public class AlterMarriedStatusIntoSingleTS  extends TransactionScript<GetCustomerByCivilStatusQuery.Criteria, AlterMarriedStatusIntoSingleTS.TransactionResult> {
    public class TransactionResult
    {
        public List<Customer> SuccessfullyAlteredCustomers = new ArrayList<Customer>();
    }

    public AlterMarriedStatusIntoSingleTS()
    {
        super(DataSynchronizationManager.GetInstance(), DataSynchronizationManager.GetInstance());
    }

    @Override
    public TransactionResult ExecutionBody() {
        IRepository<Customer> repository = GetRepositoryRegistry().GetRepository(Customer.class);
        GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = GetInput();
        IUnitOfWork uow = CreateUnitOfWork();

        List<Customer> customers = repository.Matching(criteriaByStatus);
        CustomerInvocationDelegates delegates = new CustomerInvocationDelegates();

        for (Customer customer: customers) {
            customer.Name = "Test is now single";

            try
            {
                uow.RegisterDirty(customer, delegates);
            }
            catch (NoSuchObjectException ex)
            {
            }
        }

        TransactionResult result = new TransactionResult();

        uow.Commit(new AlterMarriedInvocationDelegates(result));

        return result;
    }
}
