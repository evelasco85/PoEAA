using System;
using System.Collections.Generic;
using Framework.Data_Manipulation;
using System.Collections;

namespace Framework.Tests.CustomerServices
{
    class CustomerMapper : BaseMapper<Customer>
    {
        public const string SUCCESS_DESCRIPTION = "description";

        IDictionary<string, Customer> _internalData = new Dictionary<string, Customer>();

        public IDictionary<string, Customer> InternalData
        {
            get { return _internalData; }
        }

        public override bool Update(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData[entity.Number] = entity;

            Hashtable results = new Hashtable
            {
                {SUCCESS_DESCRIPTION, string.Format("{0}", entity.Number) }
            };

            successfulInvocation(entity, results);

            return true;
        }

        public override bool Delete(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData.Remove(entity.Number);

            Hashtable results = new Hashtable
            {
                {SUCCESS_DESCRIPTION, string.Format("{0}", entity.Number) }
            };

            successfulInvocation(entity, results);

            return true;
        }

        public override bool Insert(ref Customer entity, SuccessfulInvocationDelegate successfulInvocation,
            FailedInvocationDelegate failedInvocation)
        {
            _internalData.Add(entity.Number, entity);

            Hashtable results = new Hashtable
            {
                {SUCCESS_DESCRIPTION, string.Format("{0}", entity.Number) }
            };

            successfulInvocation(entity, results);

            return true;
        }
    }
}
