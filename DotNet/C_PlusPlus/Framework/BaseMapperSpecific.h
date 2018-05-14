#pragma once

#include "BaseMapper.h"

using namespace std;

namespace Framework
{
	namespace Domain
	{
		//Forward declaration
		class DomainObject;
	}

	namespace DataManipulation
	{
		template<typename TEntity>
		class FRAMEWORK_API BaseMapperSpecific : public BaseMapper
		{
		protected:
			BaseMapperSpecific();
			BaseMapperSpecific(BaseMapperSpecific&&) = default;
			BaseMapperSpecific& operator=(BaseMapperSpecific&&) = default;
			BaseMapperSpecific(const BaseMapperSpecific&) = delete;
			BaseMapperSpecific& operator=(const BaseMapperSpecific&) = delete;
		public:
			virtual ~BaseMapperSpecific();

			const string GetEntityTypeName();

			/*Perform overriding of base class abstract functions*/
			bool Update(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
			bool Insert(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
			bool Delete(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
			/*****************************************************/

			//abstract functions
			virtual bool Update(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Insert(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Delete(TEntity* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
		};
	}
}