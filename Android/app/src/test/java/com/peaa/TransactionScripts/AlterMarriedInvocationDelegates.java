package com.peaa.TransactionScripts;

import com.peaa.Domain.Interfaces.IDomainObject;
import com.peaa.Interfaces.UnitOfWorkAction;
import com.peaa.Interfaces.UoWInvocationDelegates;

import java.util.Hashtable;

import com.peaa.CustomerServices.Customer;

/**
 * Created by aiko on 4/8/17.
 */

public class AlterMarriedInvocationDelegates implements UoWInvocationDelegates {
    Hashtable _results;
    AlterMarriedStatusIntoSingleTS.TransactionResult _transactionResult;

    public AlterMarriedInvocationDelegates(AlterMarriedStatusIntoSingleTS.TransactionResult transactionResult) {
        _transactionResult = transactionResult;
    }

    public Hashtable GetResults() {
        return _results;
    }

    public void SetResults(Hashtable results) {
        _results = results;
    }

    public void SuccessfulUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action) {
        _transactionResult.SuccessfullyAlteredCustomers.add((Customer) domainObject);
    }

    public void FailedUoWInvocationDelegate(IDomainObject domainObject, UnitOfWorkAction action) {

    }
}
