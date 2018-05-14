#pragma once

#include <functional>
#include <type_traits>
#include <unordered_map>
#include "DllMacros.h"

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
		class FRAMEWORK_API BaseMapper
		{
		public:
			typedef void* Object;
			typedef unordered_map<string, Object> BaseMapperHashtable;
			typedef function<void(Domain::DomainObject*, BaseMapperHashtable*)> InvocationDelegate;
			typedef InvocationDelegate SuccessfulInvocationDelegate;
			typedef InvocationDelegate FailedInvocationDelegate;
		protected:
			BaseMapper() {}
			BaseMapper(BaseMapper&&) = default;
			BaseMapper& operator=(BaseMapper&&) = default;
			BaseMapper(const BaseMapper&) = delete;
			BaseMapper& operator=(const BaseMapper&) = delete;
		public:
			virtual ~BaseMapper() {}

			//Abstract functions
			virtual bool Update(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Insert(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Delete(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
		};

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