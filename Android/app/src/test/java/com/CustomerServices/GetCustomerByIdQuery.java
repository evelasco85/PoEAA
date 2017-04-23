package com.CustomerServices;

import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.DataManipulation.BaseQueryObject;
import com.DataSynchronizationManager;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * Created by aiko on 4/8/17.
 */

public class GetCustomerByIdQuery extends BaseQueryObject<Customer, GetCustomerByIdQuery.Criteria> {

    public static class Criteria {
        public int CustomerId;

        public Criteria(int customerId) {
            CustomerId = customerId;
        }
    }

    public GetCustomerByIdQuery() {
        super( GetCustomerByIdQuery.Criteria.class);
    }

    @Override
    public List<Customer> PerformSearchOperation(GetCustomerByIdQuery.Criteria criteria) {
        HashMap<Object, String> customerList = new HashMap<Object, String>();


        customerList.put(1, "Juan dela Cruz");
        customerList.put(2, "Jane Doe");
        customerList.put(3, "John Doe");

        String searchResult = customerList.get(criteria.CustomerId);
        IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper(Customer.class);
        Customer customer = new Customer(mapper);

        customer.Number = String.valueOf(criteria.CustomerId);
        customer.Name = searchResult;

        List<Customer> customerResult = new ArrayList<Customer>();

        customerResult.add(customer);

        return customerResult;
    }
}
