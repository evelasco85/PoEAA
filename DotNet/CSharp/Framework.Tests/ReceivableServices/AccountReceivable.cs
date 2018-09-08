using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework.Tests.ReceivableServices
{
    public class AccountReceivable: DomainObject
    {
        public AccountReceivable(IBaseMapper mapper)
            : base(mapper, null)
        {
        }

        [IdentityField]
        public string Number { get; set; }

        public string CustomerNumber { get; set; }

        public string Description { get; set; }
    }
}
