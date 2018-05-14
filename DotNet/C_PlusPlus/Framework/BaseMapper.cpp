#include "stdafx.h"
#include "BaseMapper.h"
#include "DomainObject.h"
#include "BaseMapperConcrete.h"

using namespace Framework::Domain;
using namespace Framework::DataManipulation;

//BaseMapper::BaseMapper() {}

//BaseMapper::~BaseMapper() {}

//template<class TEntity>
//bool BaseMapper::Update(
//	BaseMapperConcrete<TEntity>* mapper,
//	DomainObject* entity,
//	SuccessfulInvocationDelegate* successfulInvocation,
//	FailedInvocationDelegate* failedInvocation)
//{
//	if (mapper == NULL) return false;
//
//	SetSafeSuccessfulInvocator(successfulInvocation);
//	SetSafeFailureInvocator(failedInvocation);
//
//	TEntity* instance = (TEntity*)entity;
//
//	//Forward to concrete implementation
//	return mapper->ConcreteUpdate(instance, successfulInvocation, failedInvocation);
//}