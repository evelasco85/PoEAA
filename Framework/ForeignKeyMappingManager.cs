using System;
using System.Collections.Generic;
using Framework.Domain;

namespace Framework
{
    public interface IForeignKeyMappingManager
    {
        void RegisterForeignKeyMapping<TParentEntity, TChildEntity>(
            IForeignKeyMapping<TParentEntity, TChildEntity> mapping)
            where TParentEntity : IDomainObject
            where TChildEntity : IDomainObject;

        IList<TChildEntity> GetForeignKeyValues<TParentEntity, TChildEntity>(TParentEntity entity)
            where TParentEntity : IDomainObject
            where TChildEntity : IDomainObject;
    }

    public class ForeignKeyMappingManager : IForeignKeyMappingManager
    {
        static IForeignKeyMappingManager s_instance = new ForeignKeyMappingManager();

        IDictionary<string, IDictionary<string, object>> _foreignKeMapping = new Dictionary<string, IDictionary<string, object>>();

        private ForeignKeyMappingManager()
        {
        }

        public static IForeignKeyMappingManager GetInstance()
        {
            return s_instance;
        }

        public void RegisterForeignKeyMapping<TParentEntity, TChildEntity>(
            IForeignKeyMapping<TParentEntity, TChildEntity> mapping)
            where TParentEntity : IDomainObject
            where TChildEntity : IDomainObject
        {
            string parentName = typeof(TParentEntity).FullName;

            if (!_foreignKeMapping.ContainsKey(parentName))
                _foreignKeMapping.Add(parentName, new Dictionary<string, object>());

            IDictionary<string, object> maps = _foreignKeMapping[parentName];
            string childName = typeof(TChildEntity).FullName;

            if (!maps.ContainsKey(childName))
                maps.Add(childName, mapping);
        }

        public IList<TChildEntity> GetForeignKeyValues<TParentEntity, TChildEntity>(TParentEntity entity)
            where TParentEntity : IDomainObject
            where TChildEntity : IDomainObject
        {
            string parentName = typeof(TParentEntity).FullName;

            if (!_foreignKeMapping.ContainsKey(parentName))
                throw new ArgumentException(string.Format("Foreign key mapping associated to parent type '{0}' does not exists", parentName));

            IDictionary<string, object> maps = _foreignKeMapping[parentName];
            string childName = typeof(TChildEntity).FullName;

            if (!maps.ContainsKey(childName))
                throw new ArgumentException(string.Format("Foreign key mapping associated to child type '{0}' does not exists", childName));

            IForeignKeyMapping<TParentEntity, TChildEntity> fkMapping = (IForeignKeyMapping<TParentEntity, TChildEntity>)maps[childName];

            return fkMapping.GetChildEntities(entity);
        }
    }
}
