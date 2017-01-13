using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;

namespace Framework.Tests.CustomerServices
{
    class CustomerMapper : BaseMapper<Customer>
    {
        IDictionary<string, Customer> _internalData = new Dictionary<string, Customer>();
        IList<string> _sequenceDescription = new List<string>();

        public IDictionary<string, Customer> InternalData
        {
            get { return _internalData; }
        }

        public IList<string> SequenceDescription
        {
            get { return _sequenceDescription; }
        }

        public override bool Update(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData[entity.Number] = entity;
            _sequenceDescription.Add(string.Format("{0}={1}={2}", entity.Number, "Updated", entity.GetType().FullName));
            successfulInvocation(entity);

            return true;
        }

        public override bool Delete(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData.Remove(entity.Number);
            _sequenceDescription.Add(string.Format("{0}={1}={2}", entity.Number, "Deleted", entity.GetType().FullName));
            successfulInvocation(entity);

            return true;
        }

        public override bool Insert(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData.Add(entity.Number, entity);
            _sequenceDescription.Add(string.Format("{0}={1}={2}", entity.Number, "Inserted", entity.GetType().Name));
            successfulInvocation(entity);

            return true;
        }
    }
}
