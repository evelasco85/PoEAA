package TransactionScripts;

import com.Domain.Interfaces.IDomainObject;
import com.Interfaces.UnitOfWorkAction;
import com.Interfaces.UoWInvocationDelegates;

import java.util.Hashtable;
import java.util.List;

import CustomerServices.Customer;

/**
 * Created by aiko on 4/8/17.
 */

public class AlterMarriedInvocationDelegates implements UoWInvocationDelegates {
    Hashtable _results;
    List<Customer> _successfullyAlteredCustomers;

    public AlterMarriedInvocationDelegates(List<Customer> successfullyAlteredCustomers)
    {
        _successfullyAlteredCustomers = successfullyAlteredCustomers;
    }

    public Hashtable GetResults() {
        return _results;
    }

    public void SetResults(Hashtable results) {
        _results = results;
    }

    public void SuccessfulUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action) {
        _successfullyAlteredCustomers.add((Customer) domainObject);
    }

    public void FailedUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action) {

    }
}
