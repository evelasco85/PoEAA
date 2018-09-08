using Framework.Data_Manipulation;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Tests.ReceivableServices
{
    public class GetAccountReceivablesByCustomerId : BaseQueryObject<
        AccountReceivable,
        IList<AccountReceivable>,
        GetAccountReceivablesByCustomerId.Criteria>
    {
        public class Criteria : ICriteriaTag<IList<AccountReceivable>>
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

        public override IBaseMapper<AccountReceivable> GetMapper()
        {
            return null;
        }

        public override IList<AccountReceivable> PerformSearchOperation(IBaseMapper mapper, Criteria searchInput)
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
    }
}
