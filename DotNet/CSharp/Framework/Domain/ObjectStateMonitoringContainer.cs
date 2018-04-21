using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


namespace Framework.Domain
{
    public interface IObjectStateMonitoringContainer
    {
        void Monitor<TEntity>(TEntity entity)
            where TEntity : IDomainObject;
        IList<ObjectStateMonitoringContainer.ModifiedEntity> GetModifiedEntities();
    }

    public class ObjectStateMonitoringContainer : IObjectStateMonitoringContainer
    {
        public class ModifiedEntity
        {
            public IDomainObject DomainObject { get; set; }
            public IList<PropertyInfo> DifferingProperties { get; set; }
            public IDictionary<string, object> OriginalValues { get; set; }
            public IDictionary<string, object> NewValues { get; set; }

            public ModifiedEntity(IDomainObject domainObject)
            {
                DomainObject = domainObject;
                DifferingProperties = new List<PropertyInfo>();
            }
        }

        IDictionary<string, IDictionary<string, PropertyInfo>> _observableTypeProperties = new Dictionary<string, IDictionary<string, PropertyInfo>>();
        IDictionary<Guid, IDomainObject> _observableEntities = new Dictionary<Guid, IDomainObject>();
        IDictionary<Guid, IDictionary<string, object>> _originalValues = new Dictionary<Guid, IDictionary<string, object>>();

        public void Monitor<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            if (entity == null) throw new ArgumentNullException("Cannot register null value");

            RegisterTypeProperties(entity);
            RegisterObservableEntity(entity);
            RegisterOriginalEntityValues(entity);
        }

        void RegisterTypeProperties(IDomainObject domainObject)
        {
            if (domainObject == null) return;

            string typename = domainObject.GetType().FullName;

            if (!_observableTypeProperties.ContainsKey(typename))
            {
                _observableTypeProperties.Add(typename, domainObject.GetMonitoredProperties());
            }
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

        public IList<ModifiedEntity> GetModifiedEntities()
        {
            IList<ModifiedEntity> modifiedEntities = new List<ModifiedEntity>();

            foreach(KeyValuePair<Guid, IDomainObject> kvp in _observableEntities)
            {
                IDomainObject domainObject = kvp.Value;
                IDictionary<string, object> originalValues = _originalValues[kvp.Key];
                IDictionary<string, object> currentValues = domainObject.GetCurrentMonitoredPropertyValues();

                if ((originalValues.Count != currentValues.Count) ||
                    (!originalValues.Keys.All(currentValues.Keys.Contains)))
                    throw new Exception("The count of observable values are mismatched, program error");

                IDictionary<string, PropertyInfo> observableProperties = _observableTypeProperties[domainObject.GetType().FullName];
                ModifiedEntity modifiedItem = GetModifiedEntity(domainObject, observableProperties, originalValues, currentValues);

                if (modifiedItem == null) continue;

                modifiedEntities.Add(modifiedItem);
            }

            return modifiedEntities;
        }

        ModifiedEntity GetModifiedEntity(
            IDomainObject domainObject,
            IDictionary<string, PropertyInfo> observableProperties,
            IDictionary<string, object> originalValues,
            IDictionary<string, object> currentValues
            )
        {
            ModifiedEntity item = null;
            IList<PropertyInfo> differingProperties = new List<PropertyInfo>();

            foreach (KeyValuePair<string, PropertyInfo> kvp in observableProperties)
            {
                string key = kvp.Key;
                object originalValue = originalValues[key];
                object currentValue = currentValues[key];

                if(DomainObject.NotEqualPropertyValues(originalValue, currentValue))
                {
                    differingProperties.Add(kvp.Value);
                    continue;
                }
            }

            if(differingProperties.Any())
            {
                item = new ModifiedEntity(domainObject)
                {
                    DifferingProperties = differingProperties,
                    OriginalValues = originalValues,
                    NewValues = currentValues
                };
            }

            return item;
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
