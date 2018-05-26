#pragma once

using namespace std;

template<typename TMap, typename TKeyArg, typename TValue>
typename TMap::iterator EfficientAddOrUpdate(TMap& map, const TKeyArg& key, TValue& value)
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
typename TMap::iterator EfficientAddOrUpdateByReferenceWrapper(TMap& map, const TKeyArg& key, TValue& value)
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
typename TMap::mapped_type* GetValue(TMap& map, const TKeyArg& key)
{
	typename TMap::const_iterator lowerBound = map.lower_bound(key);
	const TMap::mapped_type* foundEntity = NULL;

	if ((lowerBound != map.end()) && (!(map.key_comp()(key, lowerBound->first))))
	{
		foundEntity = &lowerBound->second;
	}

	//Remove constness away(Not Recommended)
	return const_cast<TMap::mapped_type*>(foundEntity);
}

//Removes all occurances of elements in the map that have a value that returns true for 'predicate'.
template<typename TMap, typename TPredicateFunc>
void MapRemoveValues(TMap &map, TPredicateFunc predicate)
{
	for (auto it = map.begin(); it != map.end(); )
	{
		if (predicate(*it))
		{
			it = map.erase(it);
		}
		else
		{
			++it;
		}
	}
}