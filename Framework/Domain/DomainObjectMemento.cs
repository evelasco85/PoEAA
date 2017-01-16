using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Security;

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
        private readonly IDictionary<string, Tuple<PropertyInfo, object>> _properties;
        private readonly string _hash;

        public DomainObjectMemento(IDictionary<string, Tuple<PropertyInfo, object>> properties)
        {
            if(properties == null)
                throw new ArgumentNullException("'properties' parameter is required");

            _properties = properties;
            _hash = ComputeHash(properties);
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

        string ComputeHash(IDictionary<string, Tuple<PropertyInfo, object>> properties)
        {
            StringBuilder hashSet = new StringBuilder();
            IHashService service = HashService.GetInstance();

            foreach (KeyValuePair<string, Tuple<PropertyInfo, object>> kvp in properties)
            {
                object value = kvp.Value.Item2;

                if(value == null)
                    continue;

                string hashValue = Convert.ToString(value);

                hashSet.AppendLine(service.ComputeHashValue(hashValue));
            }

            //Merkel tree???
            string accumulatedHash = service.ComputeHashValue(hashSet.ToString());

            return accumulatedHash;
        }
    }
}
