using System;
using Framework.Data_Manipulation;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        IDictionary<string, PropertyInfo> GetDiffProperties(DomainObject otherDomainObject);
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
                string key = property.Key;
                object value = property.Value.GetValue(this);

                if (value == null)
                    values.Add(key, value);
                else        //Just making sure, in-case of non-primitive slipage
                    values.Add(key, DeepClone(value));
            }

            return values;
        }

        static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        public IDictionary<string, PropertyInfo> GetDiffProperties(DomainObject otherDomainObject)
        {
            if (otherDomainObject == null) throw new ArgumentNullException("Cannot compare null value");

            if (GetType().FullName != otherDomainObject.GetType().FullName)
                throw new ArgumentException("Argument is not a comparable type");

            IDictionary<string, PropertyInfo> properties = GetMonitoredProperties();
            IDictionary<string, PropertyInfo> diffProperties = new Dictionary<string, PropertyInfo>();

            foreach (KeyValuePair<string, PropertyInfo> property in properties)
            {
                object thisValue = property.Value.GetValue(this);
                object thatValue = property.Value.GetValue(otherDomainObject);

                if(NotEqualPropertyValues(thisValue, thatValue))
                    diffProperties.Add(property);
            }

            return diffProperties;
        }

        public static bool NotEqualPropertyValues(object propVal1, object propVal2)
        {
            //Can be further improved...
            return !object.Equals(propVal1, propVal2);
        }
    }
}
