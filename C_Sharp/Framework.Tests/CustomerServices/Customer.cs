using System.Collections.Generic;
using Framework.Data_Manipulation;
using Framework.Domain;
using Framework.Tests.Mapping;
using Framework.Tests.ReceivableServices;

namespace Framework.Tests.CustomerServices
{
    public class Customer : DomainObject
    {
        public Customer(IBaseMapper mapper) : base(mapper)
        {
        }

        [IdentityField]
        public string Number { get; set; }

        public string Name { get; set; }
    }
}
