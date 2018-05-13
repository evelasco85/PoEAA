#include "stdafx.h"
#include "BaseMapper.h"

using namespace std;
using namespace Framework::DataManipulation;

BaseMapper::BaseMapper() = default;
BaseMapper::~BaseMapper() = default;

template<typename TEntity>
BaseMapperSpecific<TEntity>::BaseMapperSpecific() : BaseMapper()
{
	// Compile-time check
	static_assert(is_base_of<DomainObject, TEntity>::value, "TEntity must derive from DomainObject");
}

template<typename TEntity>
BaseMapperSpecific<TEntity>::~BaseMapperSpecific() = default;

template<typename TEntity>
const string BaseMapperSpecific<TEntity>::GetEntityTypeName()
{
	return typeid(TEntity).name();
}

template<typename TEntity>
bool BaseMapperSpecific<TEntity>::Update(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	SetSafeSuccessfulInvocator(successfulInvocation);
	SetSafeFailureInvocator(failedInvocation);

	TEntity* instance = (TEntity*)entity;

	//Forward to concrete implementation
	return Update(instance, successfulInvocation, failedInvocation);
}

template<typename TEntity>
bool BaseMapperSpecific<TEntity>::Insert(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	SetSafeSuccessfulInvocator(successfulInvocation);
	SetSafeFailureInvocator(failedInvocation);

	TEntity* instance = (TEntity*)entity;

	//Forward to concrete implementation
	return Insert(instance, successfulInvocation, failedInvocation);
}

template<typename TEntity>
bool BaseMapperSpecific<TEntity>::Delete(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	SetSafeSuccessfulInvocator(successfulInvocation);
	SetSafeFailureInvocator(failedInvocation);

	TEntity* instance = (TEntity*)entity;

	//Forward to concrete implementation
	return Delete(instance, successfulInvocation, failedInvocation);
}