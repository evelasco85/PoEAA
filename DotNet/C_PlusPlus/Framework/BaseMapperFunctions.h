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
		/*Type definifion visibility on Framework:DataManipulation namespace*/
		typedef void* Object;
		typedef unordered_map<string, Object> BaseMapperHashtable;
		typedef function<void(const Domain::DomainObject&, const BaseMapperHashtable&)> InvocationDelegate;
		typedef InvocationDelegate SuccessfulInvocationDelegate;
		typedef InvocationDelegate FailedInvocationDelegate;
		/********************************************************************/

		template<typename TOut>
		const TOut* GetResultValue(const BaseMapperHashtable& resultsTable, const string& key)
		{
			TOut* result = NULL;
			////////////////////
			return result;
		}
	}
}