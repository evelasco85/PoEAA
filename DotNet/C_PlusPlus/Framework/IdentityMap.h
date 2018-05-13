#pragma once

#include <string>
#include <map>

using namespace std;

namespace Framework
{
	class Guid;

	template<typename TEntity>
	class IdentityMap
	{
	private:
		typedef HashDictionary map<string, Guid>;
		typedef SearchDictionary map<string, TEntity>;

	private:
		HashDictionary m_hashToGuidDictionary;
		SearchDictionary m_currentSearchDictionary;

	public:
		IdentityMap() {}
	};
}