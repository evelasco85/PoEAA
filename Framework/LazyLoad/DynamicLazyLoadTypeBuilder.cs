using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Framework.Domain;

namespace Framework.LazyLoad
{
    public interface IDynamicLazyLoadTypeBuilder
    {
        Type CreateLazyLoadWrapperType<TEntity, TSearchInput>(ModuleBuilder module)
            where TEntity : IDomainObject;
    }

    public class DynamicDynamicLazyLoadTypeBuilder : IDynamicLazyLoadTypeBuilder
    {
        static IDynamicLazyLoadTypeBuilder s_instance = new DynamicDynamicLazyLoadTypeBuilder();
        
        
        public static IDynamicLazyLoadTypeBuilder GetInstance()
        {
            return s_instance;
        }

        DynamicDynamicLazyLoadTypeBuilder()
        {
           
        }
        
        public Type CreateLazyLoadWrapperType<TEntity, TSearchInput>(ModuleBuilder module)
            where TEntity : IDomainObject
        {
            TEntity entity = default(TEntity);
            Type repositoryType = typeof(IRepository<TEntity>);
            Type criteriaType =  typeof(TSearchInput);
            Type entityType = typeof(TEntity);
            Type[] ctorParams = new Type[]
            {
                repositoryType,
                criteriaType,
            };
            Type baseType = typeof(TEntity);
            TypeBuilder wrapperTypeBld = module.DefineType(
                string.Format("{0}_{1}", entityType.Name, "LazyLoadWrapper"),
                TypeAttributes.Public, 
                baseType);
            FieldBuilder repositoryField = wrapperTypeBld.DefineField("_repository", repositoryType, FieldAttributes.Private);
            FieldBuilder criteriaField = wrapperTypeBld.DefineField("_criteria", criteriaType, FieldAttributes.Private);
            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);
            ConstructorBuilder wrapperCtor = wrapperTypeBld.DefineConstructor(
                     MethodAttributes.Public,
                     CallingConventions.Standard,
                     ctorParams);

            ILGenerator ctorIL = wrapperCtor.GetILGenerator();
            
            //Send 'this' object
            ctorIL.Emit(OpCodes.Ldarg_0);

            //default base class
            ctorIL.Emit(OpCodes.Call, objCtor);

            //Load 1st param
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, repositoryField);

            //Load 2nd param
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_2);
            ctorIL.Emit(OpCodes.Stfld, criteriaField);

            //Constructor complete
            ctorIL.Emit(OpCodes.Ret);

            Type wrapperType = wrapperTypeBld.CreateType();

            return wrapperType;
        }

        void LoadTemplate<TDomainEntity, TDomainSearchInput>(out TDomainEntity entity, ref IRepository<TDomainEntity> repository, ref TDomainSearchInput criteria)
                where TDomainEntity : IDomainObject
        {
            entity = default(TDomainEntity);

            if ((repository == null) || (criteria == null))
                return;

            IList<TDomainEntity> resultList = repository.Matching(criteria);

            if ((resultList != null) && (resultList.Any()))
                entity = resultList.FirstOrDefault();

            //Perform reset, ignore lazy load on next run
            repository = null;
            criteria = default(TDomainSearchInput);
        }
    }
}
