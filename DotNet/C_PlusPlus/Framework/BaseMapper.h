#pragma once

#include "stdafx.h"
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
		typedef void* Object;
		typedef unordered_map<string, Object> BaseMapperHashtable;
		typedef function<void(Domain::DomainObject*, BaseMapperHashtable*)> InvocationDelegate;
		typedef InvocationDelegate SuccessfulInvocationDelegate;
		typedef InvocationDelegate FailedInvocationDelegate;

		class FRAMEWORK_API BaseMapper
		{
		protected:
			BaseMapper();
			BaseMapper(BaseMapper&&) = default;
			BaseMapper& operator=(BaseMapper&&) = default;
			BaseMapper(const BaseMapper&) = delete;
			BaseMapper& operator=(const BaseMapper&) = delete;
		public:
			virtual ~BaseMapper();

			//Abstract functions
			virtual bool Update(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Insert(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
			virtual bool Delete(Domain::DomainObject* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation) = 0;
		};
	}
}