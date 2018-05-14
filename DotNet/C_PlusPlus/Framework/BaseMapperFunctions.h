#pragma once

#include "stdafx.h"
#include <functional>
#include "BaseMapper.h"

using namespace std;

namespace Framework
{
	namespace Domain
	{
		//Forward declaration
		class DomainObject;
	}

	namespace DataManipulation
	{
		template < typename T >
		struct is_derived_from_DomainObject : conditional< is_base_of<Domain::DomainObject, T>::value,
			true_type, false_type >::type {};

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
				*successfulInvocation = [](Domain::DomainObject*, BaseMapperHashtable*) {};
			}
		}

		static void SetSafeFailureInvocator(FailedInvocationDelegate* failedInvocation)
		{
			if (failedInvocation == NULL)
			{
				*failedInvocation = [](Domain::DomainObject*, BaseMapperHashtable*) {};
			}
		}
	}
}