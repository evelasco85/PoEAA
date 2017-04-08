package CustomerServices;


import com.DataManipulation.BaseMapper;
import com.DataManipulation.BaseMapperInterfaces.InvocationDelegates;

import java.util.HashMap;
import java.util.Hashtable;

/**
 * Created by aiko on 4/8/17.
 */

public class CustomerMapper extends BaseMapper<Customer> {

    public static String SUCCESS_DESCRIPTION = "description";

    HashMap<String, Customer> _internalData = new HashMap<>();

    public HashMap<String, Customer> GetInternalData()
    {
        return _internalData;
    }

    public CustomerMapper()
    {
        super(Customer.class);
    }

    @Override
    public boolean ConcreteInsert(Customer customer, InvocationDelegates invocationDelegates) {
        if(_internalData.containsKey(customer.Number))
            _internalData.remove(customer.Number);

        _internalData.put(customer.Number, customer);

        Hashtable results = new Hashtable();

        results.put(SUCCESS_DESCRIPTION, String.format("Service container with key '%s' not found.", customer.Number));

        invocationDelegates.SetResults(results);
        invocationDelegates.SuccessfulInvocation(customer);

        return true;
    }

    @Override
    public boolean ConcreteUpdate(Customer customer, InvocationDelegates invocationDelegates) {
        if(_internalData.containsKey(customer.Number))
            _internalData.remove(customer.Number);

        _internalData.put(customer.Number, customer);

        Hashtable results = new Hashtable();

        results.put(SUCCESS_DESCRIPTION, String.format("Service container with key '%s' not found.", customer.Number));

        invocationDelegates.SetResults(results);
        invocationDelegates.SuccessfulInvocation(customer);

        return true;
    }

    @Override
    public boolean ConcreteDelete(Customer customer, InvocationDelegates invocationDelegates) {
        if(_internalData.containsKey(customer.Number))
            _internalData.remove(customer.Number);

        Hashtable results = new Hashtable();

        results.put(SUCCESS_DESCRIPTION, String.format("Service container with key '%s' not found.", customer.Number));

        invocationDelegates.SetResults(results);
        invocationDelegates.SuccessfulInvocation(customer);

        return true;
    }
}
