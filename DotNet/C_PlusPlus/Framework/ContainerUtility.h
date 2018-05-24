#pragma once

using namespace std;

template<typename TMap, typename TKeyArg, typename TValue>
typename TMap::iterator EfficientAddOrUpdate(TMap& map, const TKeyArg& key, const TValue& value)
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

template<typename TMap, typename TKeyArg, typename TValue>
typename TMap::iterator EfficientAddOrUpdateByRef(TMap& map, const TKeyArg& key, TValue& value)
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

template<typename TMap, typename TKeyArg>
typename const TMap::mapped_type* GetValue(TMap& map, const TKeyArg& key)
{
	typename TMap::const_iterator lowerBound = map.lower_bound(key);
	const TMap::mapped_type* foundEntity = NULL;

	if ((lowerBound != map.end()) && (!(map.key_comp()(key, lowerBound->first))))
	{
		foundEntity = &lowerBound->second;
	}

	return foundEntity;
}