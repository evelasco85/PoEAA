#pragma once

#include "stdafx.h"
#include "DllMacros.h"
#include <functional>
#include <unordered_map>

using namespace std;

namespace Framework
{
	namespace DataManipulation
	{
		class DomainObject;

		typedef void* Object;
		typedef unordered_map<string, Object> BaseMapperHashtable;
		typedef function<void(DomainObject*, BaseMapperHashtable*)> InvocationDelegate;
		typedef InvocationDelegate SuccessfulInvocationDelegate;
		typedef InvocationDelegate FailedInvocationDelegate;

		/*template < typename T >
		struct is_derived_from_DomainObject : conditional< is_base_of<DomainObject, T>::value,
			true_type, false_type >::type {};*/

		template<typename TOut>
		const TOut* GetResultValue(const BaseMapperHashtable& resultsTable, const string& key)
		{
			TOut* result = NULL;
			////////////////////
			return result;
		}

		static void SetSafeSuccessfulInvocator(SuccessfulInvocationDelegate* successfulInvocation)
		{
			if (successfulInvocation == NULL)
			{
				*successfulInvocation = [](DomainObject*, BaseMapperHashtable*) {};
			}
		}

		static void SetSafeFailureInvocator(FailedInvocationDelegate* failedInvocation)
		{
			if (failedInvocation == NULL)
			{
				*failedInvocation = [](DomainObject*, BaseMapperHashtable*) {};
			}
		}
	}
}