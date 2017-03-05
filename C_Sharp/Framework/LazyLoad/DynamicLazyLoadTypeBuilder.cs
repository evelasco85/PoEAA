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
        

        //Create a decorator pattern implementation for TEntity
        //decorator will invoice 'LoadTemplate(...)'
        public Type CreateLazyLoadWrapperType<TEntity, TSearchInput>(ModuleBuilder module)
            where TEntity : IDomainObject
        {
            TEntity entity = default(TEntity);
            Type entityType = typeof(TEntity);
            TypeBuilder wrapperTypeBld = module.DefineType(string.Format("{0}_{1}", entityType.Name, "LazyLoadWrapper"), TypeAttributes.Public);
            FieldBuilder entityField = wrapperTypeBld.DefineField("_entity", entityType, FieldAttributes.Private);

            /*
             * notes
             * http://stackoverflow.com/questions/20174913/using-reflection-to-define-property-with-typeof-nested-type
             * http://stackoverflow.com/questions/9623797/c-sharp-call-and-return-an-object-from-a-static-method-in-il
             * https://msdn.microsoft.com/en-us/library/system.reflection.emit.propertybuilder%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
             * 
             */
            AddConstructor<TEntity, TSearchInput>(ref wrapperTypeBld);
            AddProperties<TEntity>(wrapperTypeBld, entityField);


            Type wrapperType = wrapperTypeBld.CreateType();

            return wrapperType;
        }

        void AddConstructor<TEntity, TSearchInput>(ref TypeBuilder wrapperTypeBld)
            where TEntity : IDomainObject
        {
            Type repositoryType = typeof(IRepository<TEntity>);
            Type criteriaType = typeof(TSearchInput);
            Type[] ctorParams = new Type[]
            {
                repositoryType,
                criteriaType,
            };

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
        }

        void AddProperties<TEntity>(TypeBuilder wrapperTypeBld, FieldBuilder entityField)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            Type entityType = typeof(TEntity);
            List<PropertyBuilder> properties = new List<PropertyBuilder>();

            properties = entityType
                .GetProperties(flags)
                .Where(property => (!property.PropertyType.IsClass) || (property.PropertyType == typeof(string)))
                .Select(property =>
                {
                    FieldBuilder fieldBuilder = null;
                    string name = property.Name;
                    Type type = property.PropertyType;
                    PropertyBuilder propertyBuilder = wrapperTypeBld.DefineProperty(
                        name,
                        PropertyAttributes.HasDefault,
                        type,
                        null);

                    if(property.CanRead && (property.GetGetMethod(true).IsPublic))
                        ConstructGetMethod(ref propertyBuilder, wrapperTypeBld, fieldBuilder, name, type);

                    if (property.CanWrite && (property.GetSetMethod(true).IsPublic))
                        ConstructSetMethod(ref propertyBuilder, wrapperTypeBld, fieldBuilder, name, type);

                    return propertyBuilder;
                })
                .ToList();
        }


        void ConstructGetMethod(ref PropertyBuilder propertyBuilder, TypeBuilder wrapperTypeBld, FieldBuilder fieldBldr, string propertyName, Type propertyType)
        {
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            MethodBuilder getPropMthdBldr = wrapperTypeBld.DefineMethod(
                string.Format("get_{0}", propertyName),
                getSetAttr,
                propertyType,
                Type.EmptyTypes);

            ILGenerator getMethodIL = getPropMthdBldr.GetILGenerator();

            //Equivalent to: return this.fieldBldr;
            getMethodIL.Emit(OpCodes.Ldarg_0);
            getMethodIL.Emit(OpCodes.Ldfld, fieldBldr);
            getMethodIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
        }

        void ConstructSetMethod(ref PropertyBuilder propertyBuilder, TypeBuilder wrapperTypeBld, FieldBuilder fieldBldr, string propertyName, Type propertyType)
        {
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            MethodBuilder setPropMthdBldr =
                wrapperTypeBld.DefineMethod(
                    string.Format("set_{0}", propertyName),
                    getSetAttr,
                    null,
                    new Type[] { propertyType });

            ILGenerator setMethodIL = setPropMthdBldr.GetILGenerator();

            //Equivalent to: this.fieldBldr = value; return;
            //where OpCodes.Ldarg_1 is 'value'
            setMethodIL.Emit(OpCodes.Ldarg_0);
            setMethodIL.Emit(OpCodes.Ldarg_1);
            setMethodIL.Emit(OpCodes.Stfld, fieldBldr);
            setMethodIL.Emit(OpCodes.Ret);

            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }

        static void LoadTemplate<TDomainEntity, TDomainSearchInput>(out TDomainEntity entity, ref IRepository<TDomainEntity> repository, ref TDomainSearchInput criteria)
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
