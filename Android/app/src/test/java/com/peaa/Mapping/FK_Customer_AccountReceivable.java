package com.peaa.Mapping;

import com.peaa.DataSynchronizationManager;
import com.peaa.Domain.ForeignKeyMapping;
import com.peaa.Interfaces.IDataSynchronizationManager;
import com.peaa.Interfaces.IRepository;

import java.util.List;

import com.peaa.CustomerServices.Customer;
import com.peaa.ReceivableServices.AccountReceivable;
import com.peaa.ReceivableServices.GetAccountReceivablesByCustomerId;

/**
 * Created by aiko on 4/8/17.
 */

public class FK_Customer_AccountReceivable extends ForeignKeyMapping<Customer, AccountReceivable> {
    private IDataSynchronizationManager _manager;

    public FK_Customer_AccountReceivable()
    {
        _manager = DataSynchronizationManager.GetInstance();
    }

    @Override
    public List<AccountReceivable> RetrieveForeignKeyEntities(Customer parent) {
        IRepository<AccountReceivable> repository = _manager.GetRepository(AccountReceivable.class);
        GetAccountReceivablesByCustomerId.Criteria criteria = new GetAccountReceivablesByCustomerId.Criteria(parent.Number);

        return repository.Matching(criteria);
    }
}
