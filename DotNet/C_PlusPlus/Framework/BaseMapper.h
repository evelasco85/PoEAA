#pragma once

#include <functional>
#include "DllMacros.h"
#include "BaseMapperFunctions.h"

using namespace std;

class DomainObject;

namespace Framework
{
	namespace DataManipulation
	{
		class FRAMEWORK_API BaseMapper
		{
		protected:
			BaseMapper();
			BaseMapper(BaseMapper&&) = default;
			BaseMapper& operator=(BaseMapper&&) = default;
			BaseMapper(const BaseMapper&) = delete;
			BaseMapper& operator=(const BaseMapper&) = delete;
		public:
			virtual ~BaseMapper() = 0;

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
			virtual ~BaseMapperSpecific() = 0;

			const string GetEntityTypeName();

			//Perform overriding of base class abstract functions
			bool Update(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
			bool Insert(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
			bool Delete(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);

			//abstract functions
			virtual bool Update(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Insert(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Delete(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
		};
	}
}