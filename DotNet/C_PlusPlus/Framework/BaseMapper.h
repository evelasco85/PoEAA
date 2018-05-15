#pragma once

#include "stdafx.h"
#include <functional>
#include <type_traits>
#include <unordered_map>

#include "DllMacros.h"
#include "BaseMapperFunctions.h"

using namespace std;

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
		class FORCE_API_EXPORT BaseMapper
		{
		protected:
			BaseMapper() {}
		public:
			BaseMapper(BaseMapper&&) = default;
			BaseMapper& operator=(BaseMapper&&) = default;
			BaseMapper(const BaseMapper&) = delete;
			BaseMapper& operator=(const BaseMapper&) = delete;
			virtual ~BaseMapper() {}

			/*Abstract function declarations*/
			virtual bool Insert(Domain::DomainObject* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation) = 0;
			virtual bool Update(Domain::DomainObject* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation) = 0;
			virtual bool Delete(Domain::DomainObject* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation) = 0;
			/********************/
		};
	}
}