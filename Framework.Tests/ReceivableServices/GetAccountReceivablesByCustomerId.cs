using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;

namespace Framework.Tests.ReceivableServices
{
    public class GetAccountReceivablesByCustomerId : BaseQueryObject<AccountReceivable, GetAccountReceivablesByCustomerId.Criteria, IList<AccountReceivable>>
    {
        public class Criteria
        {
            public string CustomerId { get; set; }

            private Criteria()
            {
            }

            public static Criteria SearchById(string id)
            {
                return new Criteria { CustomerId = id };
            }
        }

        public override IList<AccountReceivable> PerformSearchOperation(GetAccountReceivablesByCustomerId.Criteria searchInput)
        {
            IList<AccountReceivable> accountReceivables = new List<AccountReceivable>
            {
                new AccountReceivable(null) { CustomerNumber = "1", Number = "01"},
                new AccountReceivable(null) { CustomerNumber = "3", Number = "02"},
                new AccountReceivable(null) { CustomerNumber = "1", Number = "03"},
                new AccountReceivable(null) { CustomerNumber = "3", Number = "04"},
                new AccountReceivable(null) { CustomerNumber = "5", Number = "05"},
                new AccountReceivable(null) { CustomerNumber = "1", Number = "06"},
                new AccountReceivable(null) { CustomerNumber = "5", Number = "07"},
            };

            return accountReceivables.Where(receivable => receivable.CustomerNumber == searchInput.CustomerId).ToList();
        }

        public override IList<AccountReceivable> ConstructOutput(IList<AccountReceivable> searchResult)
        {
            return searchResult;
        }
    }
}
