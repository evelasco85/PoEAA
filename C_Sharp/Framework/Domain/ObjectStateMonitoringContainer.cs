using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.Domain
{
    public interface IObjectStateMonitoringContainer
    {
        void Monitor<TEntity>(TEntity entity)
            where TEntity : IDomainObject;
    }

    public class ObjectStateMonitoringContainer : IObjectStateMonitoringContainer
    {
        IDictionary<Guid, IDomainObject> _observableEntities = new Dictionary<Guid, IDomainObject>();
        IDictionary<Guid, IDictionary<string, object>> _originalValues = new Dictionary<Guid, IDictionary<string, object>>();

        public void Monitor<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            if (entity == null) throw new ArgumentNullException("Cannot register null value");

            RegisterObservableEntity(entity);
            RegisterOriginalEntityValues(entity);
        }

        void RegisterObservableEntity(IDomainObject domainObject)
        {
            if (domainObject == null) return;

            Guid guid = domainObject.SystemId;

            if (!_observableEntities.ContainsKey(guid))
            {
                _observableEntities.Add(guid, domainObject);
            }
        }

        void RegisterOriginalEntityValues(IDomainObject domainObject)
        {
            if(domainObject == null) return;

            if (_originalValues.ContainsKey(domainObject.SystemId)) _originalValues.Remove(domainObject.SystemId);

            _originalValues
                .Add(
                    domainObject.SystemId,
                    domainObject.GetCurrentMonitoredPropertyValues()
                );
        }

        void GetModifiedObjects()
        {

        }

        void UndoChanges()//All or specific
        {

        }

       

        void ApplyChangesToContainer() //All or specific
        {

        }

        void HasChanges()
        {

        }
    }
}
