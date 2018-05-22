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
		typedef unordered_map<string, string> BaseMapperHashtable;
		typedef function<void(const Domain::DomainObject*, const BaseMapperHashtable*)> InvocationDelegate;
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

		template<typename TMap, typename TKeyArg, typename TValueArg>
		typename TMap::iterator EfficientAddOrUpdate(TMap& map, const TKeyArg& key, const TValueArg& value)
		{
			typename TMap::iterator lowerBound = map.lower_bound(key);

			if ((lowerBound != map.end()) && (!(map.key_comp()(key, lowerBound->first))))
			{
				lowerBound->second = value;

				return lowerBound;
			}
			else
			{
				typedef typename TMap::value_type MVT;

				return map.insert(lowerBound, MVT(key, value));
			}
		}

		template<typename TMap, typename TKeyArg, typename TValueArg>
		typename TMap::iterator EfficientAddOrUpdateByRef(TMap& map, const TKeyArg& key, TValueArg& value)
		{
			typename TMap::iterator lowerBound = map.lower_bound(key);

			if ((lowerBound != map.end()) && (!(map.key_comp()(key, lowerBound->first))))
			{
				lowerBound->second = value;

				return lowerBound;
			}
			else
			{
				typedef typename TMap::value_type MVT;

				return map.insert(lowerBound, MVT(key, ref(value)));
			}
		}
	}
}