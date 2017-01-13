using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;
using Framework.Domain;

namespace Framework
{
    public interface IUnitOfWork
    {
        void ObserveEntityForChanges<TEntity>(TEntity entity)
            where TEntity : IDomainObject;
        void Commit(SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);
    }

    class UnitOfWork : IUnitOfWork
    {
        delegate bool OperationDelegate(ref IDomainObject entity, SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation);

        IDictionary<Guid, IDomainObject> _observedDomainObjects = new Dictionary<Guid, IDomainObject>();

        public void ObserveEntityForChanges<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            if(_observedDomainObjects.ContainsKey(entity.SystemId))
                return;

            _observedDomainObjects.Add(entity.SystemId, entity);
        }

        public void Commit(SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation)
        {
            ApplyOperation(DomainObjectState.Manually_Created, _observedDomainObjects, successfulInvocation, failedInvocation);
            ApplyOperation(DomainObjectState.Dirty, _observedDomainObjects, successfulInvocation, failedInvocation);
            ApplyOperation(DomainObjectState.For_DataSource_Deletion, _observedDomainObjects, successfulInvocation, failedInvocation);
        }

        void ApplyOperation(
            DomainObjectState affectedStates, IDictionary<Guid, IDomainObject> domainObjects,
            SuccessfulInvocationDelegate successfulInvocation, FailedInvocationDelegate failedInvocation
            )
        {
            List<IDomainObject> affectedEntities = domainObjects
                .Values
                .Where(objects => objects.GetCurrentState() == affectedStates)
                .OrderBy(objects => ((ISystemManipulation)objects).GetTicksUpdated())
                .ToList();

            for (int index = 0; index < affectedEntities.Count; index++)
            {
                IDomainObject entity = affectedEntities[index];

                if(entity == null)
                    continue;

                IBaseMapper mapper = entity.Mapper;
                OperationDelegate operation = GetOperation(affectedStates, mapper);
                bool success = operation(ref entity, successfulInvocation, failedInvocation);

                if(success)
                    ((ISystemManipulation)entity).MarkAsClean();
            }
        }

        OperationDelegate GetOperation(DomainObjectState state, IBaseMapper mapper)
        {
            OperationDelegate operation = null;

            switch (state)
            {
                case DomainObjectState.Manually_Created:
                    operation = new OperationDelegate(mapper.Insert);
                    break;
                case DomainObjectState.Dirty:
                    operation = new OperationDelegate(mapper.Update);
                    break;
                case DomainObjectState.For_DataSource_Deletion:
                    operation = new OperationDelegate(mapper.Delete);
                    break;
            }

            return operation;
        }
    }
}
