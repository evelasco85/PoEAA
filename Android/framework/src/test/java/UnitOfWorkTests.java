import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.DataManipulation.BaseQueryObjectInterfaces.IBaseQueryObjectConcrete;
import com.DataSynchronizationManager;
import com.Interfaces.IDataSynchronizationManager;
import com.Interfaces.IRepository;
import com.Interfaces.IUnitOfWork;
import com.UnitOfWork;

import java.rmi.NoSuchObjectException;
import java.util.ArrayList;
import java.util.List;

import CustomerServices.Customer;
import CustomerServices.CustomerInvocationDelegates;
import CustomerServices.CustomerMapper;
import CustomerServices.GetCustomerByCivilStatusQuery;
import CustomerServices.GetCustomerByIdQuery;
import CustomerServices.CustomerUoWDelegate;

import static org.junit.Assert.assertEquals;

/**
 * Created by aiko on 4/8/17.
 */

public class UnitOfWorkTests {
    private IDataSynchronizationManager _manager;
    private CustomerMapper _mapper = new CustomerMapper();

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
    public void TestCommit_SimpleInsertion()
    {
        IRepository<Customer> repository = _manager.GetRepository(Customer.class);
        GetCustomerByCivilStatusQuery.Criteria criteriaByStatus = new GetCustomerByCivilStatusQuery.Criteria(GetCustomerByCivilStatusQuery.CivilStatus.Married);
        GetCustomerByIdQuery.Criteria criteriaById = new GetCustomerByIdQuery.Criteria(2);
        List<Customer> customerResults = new ArrayList<Customer>();
        IUnitOfWork uow = new UnitOfWork();

        customerResults.addAll(repository.Matching(criteriaByStatus));
        customerResults.addAll(repository.Matching(criteriaById));


        IBaseMapper mapper = DataSynchronizationManager.GetInstance().GetMapper(Customer.class);
        Customer customer1 = new Customer(mapper, "1");
        Customer customer2 = new Customer(mapper, "2");
        Customer customer3 = new Customer(mapper, "3");

        CustomerInvocationDelegates delegates = new CustomerInvocationDelegates();

        try
        {
            //Sequence of observation affects commit order
            uow.RegisterNew(customer1, delegates);
            uow.RegisterNew(customer3, delegates);
            uow.RegisterNew(customer2, delegates);
        }
        catch (NoSuchObjectException ex)
        {
        }

        List<String> sequenceDescription = new ArrayList<String>();

        uow.Commit(new CustomerUoWDelegate(sequenceDescription));

        assertEquals("1=Insert=CustomerServices.Customer", sequenceDescription.get(0));
        assertEquals("3=Insert=CustomerServices.Customer", sequenceDescription.get(1));
        assertEquals("2=Insert=CustomerServices.Customer", sequenceDescription.get(2));
    }
}
