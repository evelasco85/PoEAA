using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;

namespace Framework.Tests.CustomerServices
{
    class CustomerMapper : BaseMapper<Customer>
    {
        IDictionary<string, Customer> _internalData = new Dictionary<string, Customer>();

        public IDictionary<string, Customer> InternalData
        {
            get { return _internalData; }
        }

        public override bool Update(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData[entity.Number] = entity;
            successfulInvocation(entity);

            return true;
        }

        public override bool Delete(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData.Remove(entity.Number);
            successfulInvocation(entity);

            return true;
        }

        public override bool Insert(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData.Add(entity.Number, entity);
            successfulInvocation(entity);

            return true;
        }
    }
}
