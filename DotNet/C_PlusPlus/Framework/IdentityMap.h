#pragma once

#include <string>
#include <map>
#include "Types.h"

using namespace std;
using namespace Framework;

namespace Framework
{
	template<typename TEntity>
	class IdentityMap
	{
	public:
		typedef HashDictionary map<string, Guid>;
		typedef SearchDictionary map<string, TEntity>;

	private:
		HashDictionary m_hashToGuidDictionary;
		SearchDictionary m_currentSearchDictionary;

	public:
		IdentityMap() {}
	};
}