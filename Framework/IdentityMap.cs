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

        public TEntity GetEntity()
        {
            IList<object> values = GetValuesByFieldOrdinals(propertyInfo =>
            {
                if (!_currentSearchDictionary.ContainsKey(propertyInfo.Name))
                    return null;

                return _currentSearchDictionary[propertyInfo.Name];
            });

            string searchHash = CreateHash(values);
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
            IList<object> values = GetValuesByFieldOrdinals(propertyInfo =>
            {
                return propertyInfo.GetValue(entity);
            });

            return CreateHash(values);
        }

        IList<object> GetValuesByFieldOrdinals(Func<PropertyInfo, object> getValueFunc)
        {
            IList<object> values = new List<object>();

            if (getValueFunc == null)
                return values;

            for (int index = 0; index < _identityFields.Count; index++)
            {
                object valueObj = getValueFunc(_identityFields[index]);

                if(valueObj == null)
                    continue;

                values.Add(valueObj);
            }

            return values;
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
