package ReceivableServices;

import com.DataManipulation.BaseQueryObject;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by aiko on 4/8/17.
 */

public class GetAccountReceivablesByCustomerId extends BaseQueryObject<AccountReceivable, GetAccountReceivablesByCustomerId.Criteria> {
    public class Criteria {
        public String CustomerId;

        public Criteria(String id) {
            CustomerId = id;
        }
    }

    public GetAccountReceivablesByCustomerId()
    {
        super(AccountReceivable.class);
    }

    @Override
    public List<AccountReceivable> PerformSearchOperation(Criteria criteria) {
        List<AccountReceivable> accountReceivables = new ArrayList<AccountReceivable>();

        AccountReceivable ar1 = new AccountReceivable("1", "01", null);
        AccountReceivable ar2 = new AccountReceivable("3", "02", null);
        AccountReceivable ar3 = new AccountReceivable("1", "03", null);
        AccountReceivable ar4 = new AccountReceivable("3", "04", null);
        AccountReceivable ar5 = new AccountReceivable("5", "05", null);
        AccountReceivable ar6 = new AccountReceivable("1", "06", null);
        AccountReceivable ar7 = new AccountReceivable("5", "07", null);

        accountReceivables.add(ar1);
        accountReceivables.add(ar2);
        accountReceivables.add(ar3);
        accountReceivables.add(ar4);
        accountReceivables.add(ar5);
        accountReceivables.add(ar6);
        accountReceivables.add(ar7);

        List<AccountReceivable> results = new ArrayList<AccountReceivable>();

        for (int index = 0; index < accountReceivables.size(); ++index){
            AccountReceivable result =  accountReceivables.get(index);

            if(result.CustomerNumber == criteria.CustomerId)
                results.add(result);
        }

        return results;
    }
}
