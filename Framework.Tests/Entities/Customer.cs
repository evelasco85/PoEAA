using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain;

namespace Framework.Tests.Entities
{
    public class Customer : DomainObject
    {
        public string Number { get; set; }
        public string Name { get; set; }
    }
}
