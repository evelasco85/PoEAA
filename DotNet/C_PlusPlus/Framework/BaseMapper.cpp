#include "stdafx.h"
#include "BaseMapper.h"

using namespace std;
using namespace Framework::DataManipulation;

template<typename TEntity>
const string BaseMapperSpecific<TEntity>::GetEntityTypeName()
{
	return typeid(TEntity).name();
}

template<typename TEntity>
BaseMapperSpecific<TEntity>::BaseMapperSpecific()
{
	// Compile-time check
	static_assert(is_base_of<DomainObject, TEntity>::value, "TEntity must derive from DomainObject");
}

template<typename TEntity>
BaseMapperSpecific<TEntity>::~BaseMapperSpecific() { }
