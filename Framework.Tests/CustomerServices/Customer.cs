using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Domain;
using Framework.Tests.Mapping;
using Framework.Tests.ReceivableServices;

namespace Framework.Tests.CustomerServices
{
    public class Customer : DomainObject
    {
        FK_Customer_AccountReceivable _fk_Customer_AccountReceivable = new FK_Customer_AccountReceivable();

        public Customer(IBaseMapper mapper) : base(mapper)
        {
        }

        [IdentityField]
        public string Number { get; set; }

        public string Name { get; set; }

        public IList<AccountReceivable> AccountReceivables
        {
            get { return _fk_Customer_AccountReceivable.GetChildEntities(this); }
        }
    }
}
