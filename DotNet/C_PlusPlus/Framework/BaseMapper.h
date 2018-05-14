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
		class DomainObject;
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

			//Abstract functions
			virtual bool Update(Domain::DomainObject* entity, SuccessfulInvocationDelegate& successfulInvocation, FailedInvocationDelegate& failedInvocation) = 0;
		};
	}
}