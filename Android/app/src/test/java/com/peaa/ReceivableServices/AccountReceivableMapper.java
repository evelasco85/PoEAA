package com.peaa.ReceivableServices;

import com.peaa.DataManipulation.BaseMapper;
import com.peaa.DataManipulation.BaseMapperInterfaces.InvocationDelegates;

/**
 * Created by aiko on 4/8/17.
 */

public class AccountReceivableMapper extends BaseMapper<AccountReceivable> {
    public AccountReceivableMapper()
    {
        super(AccountReceivable.class);
    }
    @Override
    public boolean ConcreteInsert(AccountReceivable accountReceivable, InvocationDelegates invocationDelegates) {
        return false;
    }

    @Override
    public boolean ConcreteUpdate(AccountReceivable accountReceivable, InvocationDelegates invocationDelegates) {
        return false;
    }

    @Override
    public boolean ConcreteDelete(AccountReceivable accountReceivable, InvocationDelegates invocationDelegates) {
        return false;
    }
}
