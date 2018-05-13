#include "stdafx.h"
#include "BaseMapper.h"

using namespace std;
using namespace Framework::DataManipulation;

template<typename TEntity>
const string BaseMapperSpecific<TEntity>::GetEntityTypeName()
{
	return typeid(TEntity).name();
}
