using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public interface IDomainObjectMemento
    {
        IList<string> GetPropertyNames();
        object GetPropertyValue(string propertyName);
        void SetPropertyValue<TEntity>(string propertyName, ref TEntity entity);
    }

    public class DomainObjectMemento : IDomainObjectMemento
    {
        private IDictionary<string, Tuple<PropertyInfo, object>> _properties;

        public DomainObjectMemento(IDictionary<string, Tuple<PropertyInfo, object>> properties)
        {
            if(properties == null)
                throw new ArgumentNullException("'properties' parameter is required");

            _properties = properties;
        }

        public IList<string> GetPropertyNames()
        {
            return _properties.Keys.ToList();
        }

        public object GetPropertyValue(string propertyName)
        {
            return _properties[propertyName].Item2;
        }

        public void SetPropertyValue<TEntity>(string propertyName, ref TEntity entity)
        {
            PropertyInfo property = _properties[propertyName].Item1;
            object propertyValue = _properties[propertyName].Item2;

            property.SetValue(entity, propertyValue);
        }
    }
}
