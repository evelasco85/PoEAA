#pragma once

#include "stdafx.h"
#include <functional>
#include <unordered_map>

using namespace std;

namespace Framework
{
	namespace Domain
	{
		class DomainObject;
	}

	namespace DataManipulation
	{
		typedef void* Object;
		typedef unordered_map<string, Object> BaseMapperHashtable;
		typedef function<void(const Domain::DomainObject&, const BaseMapperHashtable&)> InvocationDelegate;
		typedef InvocationDelegate SuccessfulInvocationDelegate;
		typedef InvocationDelegate FailedInvocationDelegate;

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
	}
}