#pragma once

#include <functional>
#include <type_traits>
#include "DllMacros.h"
#include "DomainObject.h"
#include "BaseMapperFunctions.h"

using namespace std;
using namespace Framework::Domain;

namespace Framework
{
	namespace DataManipulation
	{
		class FRAMEWORK_API BaseMapper
		{
		protected:
			BaseMapper() {}
			BaseMapper(BaseMapper&&) = default;
			BaseMapper& operator=(BaseMapper&&) = default;
			BaseMapper(const BaseMapper&) = delete;
			BaseMapper& operator=(const BaseMapper&) = delete;
		public:
			virtual ~BaseMapper() {}

			//Abstract functions
			virtual bool Update(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Insert(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Delete(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
		};

		template<typename TEntity>
		class FRAMEWORK_API BaseMapperSpecific : public BaseMapper
		{
		protected:
			BaseMapperSpecific();
			BaseMapperSpecific(BaseMapperSpecific&&) = default;
			BaseMapperSpecific& operator=(BaseMapperSpecific&&) = default;
			BaseMapperSpecific(const BaseMapperSpecific&) = delete;
			BaseMapperSpecific& operator=(const BaseMapperSpecific&) = delete;
		public:
			virtual ~BaseMapperSpecific();

			const string GetEntityTypeName();

			/*Perform overriding of base class abstract functions*/
			bool Update(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
			{
				SetSafeSuccessfulInvocator(successfulInvocation);
				SetSafeFailureInvocator(failedInvocation);

				TEntity* instance = (TEntity*)entity;

				//Forward to concrete implementation
				return Update(instance, successfulInvocation, failedInvocation);

			}
			
			bool Insert(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
			{
				SetSafeSuccessfulInvocator(successfulInvocation);
				SetSafeFailureInvocator(failedInvocation);

				TEntity* instance = (TEntity*)entity;

				//Forward to concrete implementation
				return Insert(instance, successfulInvocation, failedInvocation);
			}

			bool Delete(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
			{
				SetSafeSuccessfulInvocator(successfulInvocation);
				SetSafeFailureInvocator(failedInvocation);

				TEntity* instance = (TEntity*)entity;

				//Forward to concrete implementation
				return Delete(instance, successfulInvocation, failedInvocation);
			}
			/*****************************************************/

			//abstract functions
			virtual bool Update(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Insert(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Delete(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
		};
	}
}