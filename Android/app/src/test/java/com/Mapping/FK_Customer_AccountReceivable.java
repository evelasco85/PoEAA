package com.Mapping;

import com.DataSynchronizationManager;
import com.Domain.ForeignKeyMapping;
import com.Interfaces.IDataSynchronizationManager;
import com.Interfaces.IRepository;

import java.util.List;

import com.CustomerServices.Customer;
import com.ReceivableServices.AccountReceivable;
import com.ReceivableServices.GetAccountReceivablesByCustomerId;

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
