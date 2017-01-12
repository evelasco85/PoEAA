using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;
using Framework.Tests.Entities;

namespace Framework.Tests.Mappers
{
    class CustomerMapper : BaseMapper<Customer>
    {
        public override void Update(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            throw new NotImplementedException();
        }

        public override void Delete(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            throw new NotImplementedException();
        }

        public override void Insert(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            throw new NotImplementedException();
        }
    }
}
