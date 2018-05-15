#pragma once

#include "BaseMapper.h"
#include "BaseMapperFunctions.h"

using namespace std;

/*
Template implementation should not be separated into .cpp and .h files, all should reside in .h files.
Every time a new type used with a template class(e.g. BaseMapperConcrete<Customer>), it recreates the entire class, replacing 'TEntity'
with 'Customer'. For this reason, it needs to know the full definition of the class (along with its methods) at compile time. It cannot
just use the class definition and delay linking until later.
*/

namespace Framework
{
	namespace Domain
	{
		/*Class forward declaration*/
		class DomainObject;
		/***************************/
	}

	namespace DataManipulation
	{

		template<class TEntity>
		class FORCE_API_EXPORT BaseMapperConcrete : public BaseMapper
		{
		protected:
			BaseMapperConcrete();
		public:
			BaseMapperConcrete(BaseMapperConcrete&&) = default;
			BaseMapperConcrete& operator=(BaseMapperConcrete&&) = default;
			BaseMapperConcrete(const BaseMapperConcrete&) = delete;
			BaseMapperConcrete& operator=(const BaseMapperConcrete&) = delete;
			virtual ~BaseMapperConcrete();

			const string GetEntityTypeName() const;

			/*Override BaseMapper abstract functions*/
			bool Update(Domain::DomainObject* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation);
			/*********************************/

			/*Abstract function declarations*/
			virtual bool ConcreteUpdate(TEntity* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation) = 0;
			/*******************************/
		};

		template<class TEntity>
		BaseMapperConcrete<TEntity>::BaseMapperConcrete() : BaseMapper()
		{
			// Compile-time type compatibility check(generic constraint in C#)
			static_assert(is_base_of<Domain::DomainObject, TEntity>::value, "TEntity must derive from DomainObject");
		}

		template<class TEntity>
		BaseMapperConcrete<TEntity>::~BaseMapperConcrete() {}

		template<class TEntity>
		const string BaseMapperConcrete<TEntity>::GetEntityTypeName() const
		{
			return typeid(TEntity).name();
		}

		template<class TEntity>
		bool BaseMapperConcrete<TEntity>::Update(Domain::DomainObject* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation)
		{
			//Forward to concrete implementation
			return ConcreteUpdate((TEntity*)entity, successfulInvocation, failedInvocation);
		}
	}
}