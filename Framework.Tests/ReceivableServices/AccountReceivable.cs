using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework.Tests.ReceivableServices
{
    public class AccountReceivable: DomainObject
    {
        public AccountReceivable(IBaseMapper mapper)
            : base(mapper)
        {
        }

        [IdentityField]
        public string Number { get; set; }

        public string CustomerNumber { get; set; }

        public string Description { get; set; }
    }
}
