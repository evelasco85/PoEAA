using System;
using Framework.Data_Manipulation;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Framework.Domain
{
    public interface ISystemManipulation
    {
        void SetQueryObject(IBaseQueryObject queryObject);
        void SetMapper(IBaseMapper mapper);
    }

    public interface IDomainObject
    {
        Guid SystemId { get; }
        IBaseMapper Mapper { get; }
        InstantiationType Instantiation { get; }

        IDictionary<string, PropertyInfo> GetMonitoredProperties();
        IDictionary<string, object> GetCurrentMonitoredPropertyValues();
    }

    public enum InstantiationType
    {
        New,
        Loaded
    }

    public class DomainObject : IDomainObject, ISystemManipulation
    {
        IBaseMapper _mapper;
        Guid _systemId;
        IBaseQueryObject _queryObject;

        public Guid SystemId
        {
            get { return _systemId; }
        }

        public IBaseMapper Mapper
        {
            get { return _mapper; }
        }

        public InstantiationType Instantiation
        {
            get
            {
                if (_queryObject == null)
                    return InstantiationType.New;
                else
                    return InstantiationType.Loaded;
            }
        }

        public DomainObject(IBaseMapper mapper)
        {
            _mapper = mapper;
            _systemId = Guid.NewGuid();
        }

        public void SetQueryObject(IBaseQueryObject queryObject)
        {
            _queryObject = queryObject;
        }

        public void SetMapper(IBaseMapper mapper)
        {
            _mapper = mapper;
        }

        public IDictionary<string, PropertyInfo> GetMonitoredProperties()
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            Type entityType = GetType();

            return entityType
                .GetProperties(flags)
                .Where(
                    property =>
                        ((property.CustomAttributes != null) &&
                        (!property
                            .CustomAttributes
                            .Any(attribute => attribute.AttributeType == typeof(IgnorePropertyMonitoringAttribute))
                        ))
                        
                        )
                .Where(property => (!property.PropertyType.IsClass) || (property.PropertyType == typeof(string)))
                .ToDictionary(property => property.Name, property => property);
        }

        public IDictionary<string, object> GetCurrentMonitoredPropertyValues()
        {
            IDictionary<string, PropertyInfo> properties = GetMonitoredProperties();
            IDictionary<string, object> values = new Dictionary<string, object>();

            foreach(KeyValuePair<string, PropertyInfo> property in properties)
            {
                values.Add(
                    property.Key,
                    property.Value.GetValue(this)
                    );
            }

            return values;
        }

        void GetDiff(DomainObject otherDomainObject)
        {

        }
    }
}
