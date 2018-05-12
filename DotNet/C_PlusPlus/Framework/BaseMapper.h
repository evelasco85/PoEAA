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
		template<typename TEntity>
		class FRAMEWORK_API BaseMapper
		{
		public:
			BaseMapper()
			{
				// Compile-time check
				static_assert(is_base_of<DomainObject, TEntity>::value, "TEntity must derive from DomainObject");
			}

			virtual ~BaseMapper() = 0;

			const string GetEntityTypeName();

			template<typename TOut>
			const TOut* GetResultValue(const BaseMapperHashtable& resultsTable, const string& key);

			bool Update(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			bool Insert(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			bool Delete(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;

			bool Update(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
			bool Insert(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
			bool Delete(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
		};
	}
}