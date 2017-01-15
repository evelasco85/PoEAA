using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework.Tests.CustomerServices
{
    public class Customer : DomainObject
    {
        public Customer(IBaseMapper mapper) : base(mapper)
        {
        }

        public string Number { get; set; }
        public string Name { get; set; }
    }
}
