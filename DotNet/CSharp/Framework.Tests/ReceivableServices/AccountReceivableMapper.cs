using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;

namespace Framework.Tests.ReceivableServices
{
    public class AccountReceivableMapper : BaseMapper<AccountReceivable>
    {
        public override bool Update(ref AccountReceivable entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            throw new NotImplementedException();
        }

        public override bool Insert(ref AccountReceivable entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(ref AccountReceivable entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            throw new NotImplementedException();
        }
    }
}
