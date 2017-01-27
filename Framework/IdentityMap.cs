using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Framework.Domain;
using Framework.Security;

namespace Framework
{
    public interface IIdentityMapQuery<TEntity>
        where TEntity : IDomainObject
    {
        IIdentityMapQuery<TEntity> SetFilter<TEntityPropertyType>(Expression<Func<TEntity, TEntityPropertyType>> keyToSearch, object keyValue);
        TEntity GetEntity();
    }

    public interface IIdentityMap
    {
        IList<PropertyInfo> GetIdentityFields();
        bool EntityHasValidIdentityFields();
        void ClearEntities();
        int Count { get; }
    }

    public interface IIdentityMap<TEntity> : IIdentityMap
        where TEntity : IDomainObject
    {
        void AddEntity(TEntity entity);
        IIdentityMapQuery<TEntity> SearchBy();
    }

    public class IdentityMap<TEntity> : 
        IIdentityMap<TEntity>,
        IIdentityMapQuery<TEntity>
        where TEntity : IDomainObject
    {
        IList<PropertyInfo> _identityFields = new List<PropertyInfo>();
        IDictionary<Guid, IDomainObject> _guidToDomainObjectDictionary = new Dictionary<Guid, IDomainObject>();
        IDictionary<string, Guid> _hashToGuidDictionary = new Dictionary<string, Guid>();
        IDictionary<string, object> _currentSearchDictionary = new Dictionary<string, object>();

        public int Count { get { return _guidToDomainObjectDictionary.Count; } }

        public IdentityMap()
        {
            _identityFields = GetIdentities();
        }

        IList<PropertyInfo> GetIdentities()
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            Type entityType = typeof(TEntity);
            
            return entityType
                .GetProperties(flags)
                .Where(
                    property =>
                        (property.CustomAttributes != null) && (property.CustomAttributes.Any()) &&
                        (property.CustomAttributes.Any(
                            attribute => attribute.AttributeType == typeof(IdentityFieldAttribute))))
                .Where(property => (!property.PropertyType.IsClass) || (property.PropertyType == typeof(string)))
                .ToList();
        }

        public IIdentityMapQuery<TEntity> SearchBy()
        {
            //Start new seach
            _currentSearchDictionary.Clear();

            return this;
        }

        public IIdentityMapQuery<TEntity> SetFilter<TEntityPropertyType>(Expression<Func<TEntity, TEntityPropertyType>> keyToSearch, object keyValue)
        {
            string entityFieldName = ((MemberExpression)keyToSearch.Body).Member.Name;
            string searchValue = Convert.ToString(keyValue);

            if(!_identityFields.Any(field => field.Name == entityFieldName))
                throw new ArgumentException(string.Format("Field '{0}' is not marked with 'IdentityFieldAttribute'", entityFieldName));

            _currentSearchDictionary.Add(entityFieldName, searchValue);

            return this;
        }

        TEntity CreateDefaultInstance()
        {
            var parameters = typeof(TEntity)
               .GetConstructors()
               .Single()
               .GetParameters()
               .Select(p => (object)null)     //Set parameter values to null
               .ToArray();
            
            return (TEntity)Activator.CreateInstance(typeof(TEntity), parameters);
        }

        void SetSearchPropertyValues(ref TEntity entity, IList<PropertyInfo> properties, IDictionary<string, object> searchDictionary)
        {
            IList<PropertyInfo> matchedProperties = properties
                .Where(field => searchDictionary.Any(search => field.Name == search.Key))
                .ToList();

            for (int index = 0; index < matchedProperties.Count; index++)
            {
                PropertyInfo property = matchedProperties[index];
                object searchValue = searchDictionary[property.Name];

                property.SetValue(entity, searchValue);
            }
        }

        public TEntity GetEntity()
        {
            TEntity searchEntity = CreateDefaultInstance();

            SetSearchPropertyValues(ref searchEntity, _identityFields, _currentSearchDictionary);

            string searchHash = CreateHash(searchEntity);
            Guid foundGuid = (_hashToGuidDictionary.ContainsKey(searchHash)) ? _hashToGuidDictionary[searchHash] : Guid.Empty;

            if (foundGuid != Guid.Empty)
                return (TEntity)_guidToDomainObjectDictionary[foundGuid];

            return default(TEntity);
        }

        public IList<PropertyInfo> GetIdentityFields()
        {
            return ((List<PropertyInfo> )_identityFields).AsReadOnly();
        }

        public bool EntityHasValidIdentityFields()
        {
            return _identityFields.Any();
        }

        public void ClearEntities()
        {
            _guidToDomainObjectDictionary.Clear();
            _hashToGuidDictionary.Clear();
        }

        public void AddEntity(TEntity entity)
        {
            if(!EntityHasValidIdentityFields())
                throw new ArgumentException("'{0}' requires declaration of 'IdentityFieldAttribute' on primitive property(ies) to use IdentityMap", typeof(TEntity).FullName);

            string hash = CreateHash(entity);
            Guid systemId = entity.SystemId;

            if (!VerifyEntityExistence(hash, systemId))
            {
                _guidToDomainObjectDictionary.Add(systemId, entity);
                _hashToGuidDictionary.Add(hash, systemId);
            }
        }

        string CreateHash(TEntity entity)
        {
            IList<object> values = new List<object>();
            
            for (int index = 0; index < _identityFields.Count; index++)
            {
                object valueObj = _identityFields[index].GetValue(entity);

                values.Add(valueObj);
            }

            return CreateHash(values);
        }

        string CreateHash(IList<object> values)
        {
            StringBuilder hashSet = new StringBuilder();
            IHashService service = HashService.GetInstance();

            for (int index = 0; index < values.Count; index++)
            {
                object valueObj = values[index];

                hashSet.AppendLine(service.ComputeHashValue(Convert.ToString(valueObj)));
            }

            string accumulatedHash = service.ComputeHashValue(hashSet.ToString());

            return accumulatedHash;
        }

        bool VerifyEntityExistence(string hash, Guid systemId)
        {
            //Verify hash dictionary
            if ((!string.IsNullOrEmpty(hash)) && (_hashToGuidDictionary.ContainsKey(hash)))
                return true;

            //Verify guid dictionary
            if (_guidToDomainObjectDictionary.ContainsKey(systemId))
            {
               FixDictionaryAnomalies(hash, systemId);

                return true;
            }
            
            return false;
        }

        void FixDictionaryAnomalies(string hash, Guid systemId)
        {
            //Anomalies found, possibly by change of primary key(s) values
            //Perform reverse-lookup
            List<KeyValuePair<string, Guid>> inconsistentHashEntries = _hashToGuidDictionary.Where(x => x.Value == systemId).ToList();

            //Update/Fix inconsistencies
            if ((inconsistentHashEntries != null) && inconsistentHashEntries.Any())
            {
                inconsistentHashEntries.ForEach(entry => _hashToGuidDictionary.Remove(entry.Key));
                _hashToGuidDictionary.Add(hash, systemId);
            }
        }
    }
}
