#pragma once

#include "stdafx.h"
#include <functional>
#include <type_traits>
#include <unordered_map>

#include "BaseMapperConcrete.h"

using namespace std;
using namespace Framework::Domain;

namespace Framework
{
	namespace Domain
	{
		class DomainObject;
	}

	namespace DataManipulation
	{
		class FRAMEWORK_API BaseMapper
		{
		public:
			BaseMapper() {}
			BaseMapper(BaseMapper&&) = default;
			BaseMapper& operator=(BaseMapper&&) = default;
			BaseMapper(const BaseMapper&) = delete;
			BaseMapper& operator=(const BaseMapper&) = delete;
			virtual ~BaseMapper() {}

			//Abstract functions
			template<class TEntity>
			bool Update(
				BaseMapperConcrete<TEntity>* mapper,
				Domain::DomainObject* entity,
				SuccessfulInvocationDelegate* successfulInvocation,
				FailedInvocationDelegate* failedInvocation);
		};

		template<class TEntity>
		bool BaseMapper::Update(
			BaseMapperConcrete<TEntity>* mapper,
			Domain::DomainObject* entity,
			SuccessfulInvocationDelegate* successfulInvocation,
			FailedInvocationDelegate* failedInvocation)
		{
			if (mapper == NULL) return false;

			SetSafeSuccessfulInvocator(successfulInvocation);
			SetSafeFailureInvocator(failedInvocation);

			TEntity* instance = (TEntity*)entity;

			//Forward to concrete implementation
			return mapper->ConcreteUpdate(instance, successfulInvocation, failedInvocation);
		}
	}
}