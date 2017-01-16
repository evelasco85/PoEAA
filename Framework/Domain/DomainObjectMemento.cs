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
        object GetPropertyValue(string propertyName);
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

        public object GetPropertyValue(string propertyName)
        {
            return _properties[propertyName].Item2;
        }
    }
}
