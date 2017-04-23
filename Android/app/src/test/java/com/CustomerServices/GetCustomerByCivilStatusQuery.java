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

public class GetCustomerByCivilStatusQuery extends BaseQueryObject<Customer, GetCustomerByCivilStatusQuery.Criteria> {
    public enum CivilStatus
    {
        Single,
        Married,
        Divorced
    }

    public static class Criteria
    {
        public CivilStatus CurrentCivilStatus;

        public Criteria(CivilStatus status)
        {
            CurrentCivilStatus = status;
        }
    }

    public class KVP
    {
        public String Key;
        public String Value;

        public KVP(String key, String value)
        {
            Key = key;
            Value = value;
        }
    }

    public GetCustomerByCivilStatusQuery(){
        super(GetCustomerByCivilStatusQuery.Criteria.class);
    }

    @Override
    public List<Customer> PerformSearchOperation(Criteria criteria) {
        HashMap<CivilStatus, KVP> customerList = new HashMap<CivilStatus, KVP>();


        customerList.put(CivilStatus.Single, new KVP("4", "Test Single"));
        customerList.put(CivilStatus.Married, new KVP("5", "Test Married"));
        customerList.put(CivilStatus.Divorced, new KVP("6", "Test Divorced"));

        KVP searchResult = customerList.get(criteria.CurrentCivilStatus);
        IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper(Customer.class);
        Customer customer = new Customer(mapper);

        customer.Number = searchResult.Key;
        customer.Name = searchResult.Value;

        List<Customer> customerResult =   new ArrayList<Customer>();

        customerResult.add(customer);

        return customerResult;
    }
}
