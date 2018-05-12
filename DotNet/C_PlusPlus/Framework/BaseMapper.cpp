#include "stdafx.h"
#include "BaseMapper.h"

using namespace std;
using namespace Framework::DataManipulation;

template<typename TEntity>
BaseMapper<TEntity>::~BaseMapper() = default;

template<typename TEntity>
const string BaseMapper<TEntity>::GetEntityTypeName()
{
	return typeid(TEntity).name();
}

template<typename TEntity>
template<typename TOut>
const TOut* BaseMapper<TEntity>::GetResultValue(const BaseMapperHashtable& resultsTable, const string& key)
{
	TOut* result = NULL;
	////////////////////
	return result;
}

template<typename TEntity>
bool BaseMapper<TEntity>::Update(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	SetSafeSuccessfulInvocator(successfulInvocation);
	SetSafeFailureInvocator(failedInvocation);

	TEntity* instance = (TEntity*)entity;

	return Update(instance, successfulInvocation, failedInvocation);
}

template<typename TEntity>
bool BaseMapper<TEntity>::Insert(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	SetSafeSuccessfulInvocator(successfulInvocation);
	SetSafeFailureInvocator(failedInvocation);

	TEntity* instance = (TEntity*)entity;

	return Insert(instance, successfulInvocation, failedInvocation);
}

template<typename TEntity>
bool BaseMapper<TEntity>::Delete(DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	SetSafeSuccessfulInvocator(successfulInvocation);
	SetSafeFailureInvocator(failedInvocation);

	TEntity* instance = (TEntity*)entity;

	return Delete(instance, successfulInvocation, failedInvocation);
}