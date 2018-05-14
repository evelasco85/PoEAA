#pragma once

#include "BaseMapper.h"
#include "BaseMapperFunctions.h"

using namespace std;

namespace Framework
{
	namespace Domain
	{
		class DomainObject;
	}

	namespace DataManipulation
	{
		template<class TEntity>
		class FORCE_API_EXPORT BaseMapperConcrete : public BaseMapper
		{
		public:
			BaseMapperConcrete();

			BaseMapperConcrete(BaseMapperConcrete&&) = default;
			BaseMapperConcrete& operator=(BaseMapperConcrete&&) = default;
			BaseMapperConcrete(const BaseMapperConcrete&) = delete;
			BaseMapperConcrete& operator=(const BaseMapperConcrete&) = delete;
			virtual ~BaseMapperConcrete();

			const string GetEntityTypeName() const
			{
				return typeid(TEntity).name();
			}

			bool Update(Domain::DomainObject* entity, SuccessfulInvocationDelegate& successfulInvocation, FailedInvocationDelegate& failedInvocation);
			virtual bool ConcreteUpdate(TEntity* entity, SuccessfulInvocationDelegate& successfulInvocation, FailedInvocationDelegate& failedInvocation) = 0;
		};

		template<class TEntity>
		BaseMapperConcrete<TEntity>::BaseMapperConcrete()
		{
			// Compile-time check
			static_assert(is_base_of<Domain::DomainObject, TEntity>::value, "TEntity must derive from DomainObject");
		}

		template<class TEntity>
		BaseMapperConcrete<TEntity>::~BaseMapperConcrete() {}

		template<class TEntity>
		bool BaseMapperConcrete<TEntity>::Update(Domain::DomainObject* entity, SuccessfulInvocationDelegate& successfulInvocation, FailedInvocationDelegate& failedInvocation)
		{
			TEntity* instance = (TEntity*)entity;

			//Forward to concrete implementation
			return ConcreteUpdate(instance, successfulInvocation, failedInvocation);
		}
	}
}