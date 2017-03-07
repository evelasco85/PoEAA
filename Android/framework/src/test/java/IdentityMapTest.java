import com.IdentityMap;
import com.Interfaces.IIdentityMapConcrete;

import CustomerServices.Customer;

import static org.junit.Assert.*;

/**
 * Created by aiko on 3/5/17.
 */
public class IdentityMapTest {
    @org.junit.Test
    public void Test_DuplicateEntry() throws Exception {
        Customer customer1 = new Customer(null);

        customer1.Number = "001";

        IIdentityMapConcrete<Customer> map = new IdentityMap<Customer>(Customer.class);

        map.AddEntity(customer1);
        map.AddEntity(customer1);

        assertEquals(1, map.GetCount());
    }

    @org.junit.Test
    public void Test_Identity_Updates()
    {
        Customer customer1 = new Customer(null);

        customer1.Name = "John Doe";
        customer1.Number = "001";

        IIdentityMapConcrete<Customer> map = new IdentityMap<Customer>(Customer.class);

        map.AddEntity(customer1);

        customer1.Number = "002";

        map.AddEntity(customer1);       //Updating primary key values

        Customer matchedCustomer = null;
        Customer notFoundCustomer = null;

        try
        {
            matchedCustomer = map.SearchBy().SetFilter("Number", "002").GetEntity();
            notFoundCustomer = map.SearchBy().SetFilter("Number", "001").GetEntity();
        }
        catch (NoSuchFieldException ex)
        {
            fail();
        }


        assertEquals(1, map.GetCount());
        assertTrue(notFoundCustomer == null);
        assertNotNull(matchedCustomer);
        assertEquals(customer1.Number, matchedCustomer.Number);
        assertEquals(customer1.Name, matchedCustomer.Name);
    }

    @org.junit.Test
    public void Test_Retrieval_By_Keys()
    {
        Customer customer1 = new Customer(null);
        Customer customer2 = new Customer(null);
        Customer customer3 = new Customer(null);
        Customer customer4 = new Customer(null);
        Customer customer5 = new Customer(null);

        customer1.Number = "001";
        customer2.Number = "002";
        customer3.Number = "003";
        customer4.Number = "004"; customer4.Name = "John Doe";
        customer5.Number = "005";

        IIdentityMapConcrete<Customer> map = new IdentityMap<Customer>(Customer.class);

        map.AddEntity(customer1);
        map.AddEntity(customer2);
        map.AddEntity(customer3);
        map.AddEntity(customer4);
        map.AddEntity(customer5);

        assertEquals(5, map.GetCount());

        Customer matchedCustomer = null;

        try
        {
            matchedCustomer =  map.SearchBy()
                    .SetFilter("Number", "004")
                    .GetEntity();
        }
        catch (NoSuchFieldException ex)
        {
            fail();
        }

        assertEquals(customer4.Number, matchedCustomer.Number);
        assertEquals(customer4.Name, matchedCustomer.Name);
    }

    @org.junit.Test
    public void Test_SearchResult_NotFound()
    {
        Customer customer1 = new Customer(null);
        Customer customer2 = new Customer(null);
        Customer customer3 = new Customer(null);
        Customer customer4 = new Customer(null);
        Customer customer5 = new Customer(null);

        customer1.Number = "001";
        customer2.Number = "002";
        customer3.Number = "003";
        customer4.Number = "004"; customer4.Name = "John Doe";
        customer5.Number = "005";

        IIdentityMapConcrete<Customer> map = new IdentityMap<Customer>(Customer.class);

        map.AddEntity(customer1);
        map.AddEntity(customer2);
        map.AddEntity(customer3);
        map.AddEntity(customer4);
        map.AddEntity(customer5);

        assertEquals(5, map.GetCount());

        Customer matchedCustomer = null;

        try
        {
            matchedCustomer =  map.SearchBy()
                    .SetFilter("Number", "007")
                    .GetEntity();
        }
        catch (NoSuchFieldException ex)
        {
            fail();
        }

        assertNull(matchedCustomer);
    }

    @org.junit.Test
    public void Test_Clear_Identities()
    {
        Customer customer1 = new Customer(null);
        Customer customer2 = new Customer(null);
        Customer customer3 = new Customer(null);
        Customer customer4 = new Customer(null);
        Customer customer5 = new Customer(null);

        customer1.Number = "001";
        customer2.Number = "002";
        customer3.Number = "003";
        customer4.Number = "004"; customer4.Name = "John Doe";
        customer5.Number = "005";

        IIdentityMapConcrete<Customer> map = new IdentityMap<Customer>(Customer.class);

        map.AddEntity(customer1);
        map.AddEntity(customer2);
        map.AddEntity(customer3);
        map.AddEntity(customer4);
        map.AddEntity(customer5);

        assertEquals(5, map.GetCount());

        map.ClearEntities();

        assertEquals(0, map.GetCount());
    }
}