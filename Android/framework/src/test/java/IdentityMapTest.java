import com.IdentityMap;
import com.Interfaces.IIdentityMapConcrete;

import CustomerServices.Customer;

import static org.junit.Assert.*;

/**
 * Created by aiko on 3/5/17.
 */
public class IdentityMapTest {
    @org.junit.Test
    public void addEntity() throws Exception {
        Customer customer1 = new Customer(null);

        customer1.Number = "001";

        IIdentityMapConcrete<Customer> map = new IdentityMap<Customer>(Customer.class);

        map.AddEntity(customer1);
        map.AddEntity(customer1);

        assertEquals(1, map.GetCount());
    }

}