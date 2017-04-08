package CustomerServices;

import com.DataManipulation.BaseMapper;
import com.Domain.Interfaces.IDomainObject;
import com.Interfaces.UnitOfWorkAction;
import com.Interfaces.UoWInvocationDelegates;

import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;

import TransactionScripts.AlterMarriedStatusIntoSingleTS;

import static CustomerServices.CustomerMapper.SUCCESS_DESCRIPTION;

/**
 * Created by aiko on 4/8/17.
 */

public class CustomerUoWDelegate implements UoWInvocationDelegates {
    Hashtable _results;
    List<String> _sequenceDescription;

    public CustomerUoWDelegate(List<String> sequenceDescription) {
        _sequenceDescription = sequenceDescription;
    }

    public Hashtable GetResults() {
        return _results;
    }

    public void SetResults(Hashtable results) {
        _results = results;
    }

    public void SuccessfulUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action) {
        String description = BaseMapper.GetHashValue(_results, CustomerMapper.SUCCESS_DESCRIPTION);

        _sequenceDescription.add(String.format("%s=%s=%s", description, action.toString(), domainObject.GetMapper().GetEntityTypeName()));
    }

    public void FailedUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action) {

    }
}
