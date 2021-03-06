﻿using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework.Tests.CustomerServices
{
    public class Customer : DomainObject
    {
        public class InnerClass { }

        public Customer(IBaseMapper mapper, IBaseQueryObject queryObject) : base(mapper, queryObject)
        {
        }

        [IdentityField]
        public string Number { get; set; }

        public string Name { get; set; }

        [IgnorePropertyMonitoring]
        public bool NotSoHelpfulMember { get; set; }

        public InnerClass Temp { get; set; }
    }
}
