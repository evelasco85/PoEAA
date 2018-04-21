﻿using System;
using System.Collections.Generic;
using Framework.Domain;
using Framework.Tests.CustomerServices;
using Framework.Tests.ReceivableServices;

namespace Framework.Tests.Mapping
{
    public class FK_Customer_AccountReceivable : ForeignKeyMapping<Customer, AccountReceivable>
    {
        private IDataSynchronizationManager _manager;

        public FK_Customer_AccountReceivable()
        {
            _manager = DataSynchronizationManager.GetInstance();
        }

        public override IList<AccountReceivable> RetrieveForeignKeyEntities(Customer parent)
        {
            IRepository<AccountReceivable> repository = _manager.GetRepository<AccountReceivable>();
            GetAccountReceivablesByCustomerId.Criteria criteria = GetAccountReceivablesByCustomerId.Criteria.SearchById(parent.Number);

            return repository.Matching(criteria);
        }
    }
}
