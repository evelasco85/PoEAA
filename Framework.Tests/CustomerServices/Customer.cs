using Framework.Domain;

namespace Framework.Tests.CustomerServices
{
    public class Customer : DomainObject
    {
        public string Number { get; set; }
        public string Name { get; set; }
    }
}
