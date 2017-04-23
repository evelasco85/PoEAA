package com;

import com.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.Interfaces.IDataSynchronizationManager;

import java.util.ArrayList;
import java.util.List;

import com.CustomerServices.Customer;
import com.CustomerServices.CustomerMapper;
import com.CustomerServices.GetCustomerByCivilStatusQuery;
import com.CustomerServices.GetCustomerByIdQuery;
import com.TransactionScripts.AlterMarriedStatusIntoSingleTS;

import static org.junit.Assert.assertEquals;

/**
 * Created by aiko on 4/8/17.
 */

public class TransactionScriptTest {
    private IDataSynchronizationManager _manager;

    @org.junit.Before
    public void Initialize()
    {
        _manager = DataSynchronizationManager.GetInstance();

        List<IBaseQueryObjectConcrete<Customer>> customerQueryObjects = new ArrayList<IBaseQueryObjectConcrete<Customer>>();

        customerQueryObjects.add(new GetCustomerByIdQuery());
        customerQueryObjects.add(new GetCustomerByCivilStatusQuery());
        _manager.RegisterEntity(Customer.class, new CustomerMapper(), customerQueryObjects);
    }

    @org.junit.Test
    public void TestRunScript()
    {
        AlterMarriedStatusIntoSingleTS transactionScript = new AlterMarriedStatusIntoSingleTS();

        transactionScript.SetInput(new GetCustomerByCivilStatusQuery.Criteria(GetCustomerByCivilStatusQuery.CivilStatus.Married));

        transactionScript.RunScript();

        List<Customer> resultsByStatus = transactionScript.GetOutput().SuccessfullyAlteredCustomers;

        assertEquals(1, resultsByStatus.size());
        assertEquals("5", resultsByStatus.get(0).Number);
        assertEquals("Test is now single", resultsByStatus.get(0).Name);
    }
}
