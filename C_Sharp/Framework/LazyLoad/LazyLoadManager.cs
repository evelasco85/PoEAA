using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Framework.Domain;

namespace Framework.LazyLoad
{
    public interface ILazyLoadManager
    {
        void RegisterLazyLoadType<TEntity, TSearchInput>()
            where TEntity : IDomainObject;
        TEntity CreateLazyLoadEntity<TEntity, TSearchInput>(TSearchInput criteria)
            where TEntity : IDomainObject;
    }

    public class LazyLoadManager : ILazyLoadManager
    {
        static ILazyLoadManager s_instance = new LazyLoadManager();

        AppDomain _domain = Thread.GetDomain();
        AssemblyName _assemblyName = new AssemblyName("LazyLoadAssembly");
        IDictionary<string, Type> _typeDictionary = new ConcurrentDictionary<string, Type>();
        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _wrapperModule;


        LazyLoadManager()
        {
             _assemblyBuilder = _domain.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndSave);
            _wrapperModule = _assemblyBuilder.DefineDynamicModule("LazyLoadModule", "LazyLoadEntities.dll");
        }

        public static ILazyLoadManager GetInstance()
        {
            return s_instance;
        }

        string GetFullname<TEntity, TSearchInput>()
        {
            return string.Format("{0} | {1}", typeof(TEntity).FullName, typeof(TSearchInput).FullName);
        }

        public void RegisterLazyLoadType<TEntity,TSearchInput>()
            where TEntity : IDomainObject
        {
            string fullName = GetFullname<TEntity, TSearchInput>();

            if(_typeDictionary.ContainsKey(fullName))
                return;

            _typeDictionary.Add(
                fullName,
                DynamicDynamicLazyLoadTypeBuilder.GetInstance().CreateLazyLoadWrapperType<TEntity, TSearchInput>(_wrapperModule)
                );
        }

        public TEntity CreateLazyLoadEntity<TEntity, TSearchInput>(TSearchInput criteria)
            where TEntity : IDomainObject
        {
            string fullName = GetFullname<TEntity, TSearchInput>();

            if (!_typeDictionary.ContainsKey(fullName))
                throw new TypeLoadException(string.Format("Lazy load for type '{0}' is not registered", fullName));

            Type lazyLoadType = _typeDictionary[fullName];
            TEntity entity = (TEntity) Activator.CreateInstance(
                lazyLoadType,
                new object[]
                {
                    DataSynchronizationManager.GetInstance().GetRepository<TEntity>(),
                    criteria
                });

            return entity;
        }
    }
}
