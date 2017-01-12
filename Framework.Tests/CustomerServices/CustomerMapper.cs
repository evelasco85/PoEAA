using System;
using Framework.Data_Manipulation;

namespace Framework.Tests.CustomerServices
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
