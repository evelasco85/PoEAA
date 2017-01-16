using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public interface IDomainObjectMementoService
    {
        IDomainObjectMemento CreateMemento<TEntity>(TEntity entity)
            where TEntity : IDomainObject;

        void SetMemento<TEntity>(ref TEntity entity, IDomainObjectMemento memento)
            where TEntity : IDomainObject;
    }

    public class DomainObjectMementoService : IDomainObjectMementoService
    {
        static IDomainObjectMementoService s_instance = new DomainObjectMementoService();

        private DomainObjectMementoService()
        {
        }

        public static IDomainObjectMementoService GetInstance()
        {
            return s_instance;
        }

        public IDomainObjectMemento CreateMemento<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            if (entity == null)
                throw new ArgumentNullException("'entity' parameter is required");

            IDictionary<string, Tuple<PropertyInfo, object>> properties = GetPrimitiveProperties(entity);
            DomainObjectMemento memento = new DomainObjectMemento(properties);

            return memento;
        }

        public void SetMemento<TEntity>(ref TEntity entity, IDomainObjectMemento memento)
            where TEntity : IDomainObject
        {
            if (entity == null)
                throw new ArgumentNullException("'entity' parameter is required");

            if (memento == null)
                throw new ArgumentNullException("'memento' parameter is required");

            IList<string> propertyNames = memento.GetPropertyNames();

            for (int index = 0; index < propertyNames.Count; index++)
            {
                memento.SetPropertyValue(propertyNames[index], ref entity);
            }
        }

        IDictionary<string, Tuple<PropertyInfo, object>> GetPrimitiveProperties<TEntity>(TEntity entity)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            Type entityType = typeof(TEntity);
            IList<PropertyInfo> properties = entityType.GetProperties(flags);
            IDictionary<string, Tuple<PropertyInfo, object>> propertyValues =
                new Dictionary<string, Tuple<PropertyInfo, object>>();

            for (int index = 0; index < properties.Count; index++)
            {
                PropertyInfo property = properties[index];
                object value = property.GetValue(entity);
                bool isPrimitive = (!property.PropertyType.IsClass) || (property.PropertyType == typeof(string));

                if (!isPrimitive)
                    continue;

                propertyValues.Add(property.Name, new Tuple<PropertyInfo, object>(property, value));
            }

            return propertyValues;
        }
    }
}
