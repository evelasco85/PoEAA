#pragma once

#include "stdafx.h"
#include "BaseMapper.h"
#include "BaseMapperFunctions.h"
#include "Customer.h"
#include <memory>
#include <unordered_map>

using namespace std;
using namespace CustomerServices;
using namespace Framework::DataManipulation;

namespace CustomerServices
{
	class CustomerMapper : public BaseMapperSpecific<Customer>
	{
	private:
		class Implementation;
		unique_ptr<Implementation> pImpl;
	public:
		CustomerMapper() :
			BaseMapperSpecific<Customer>(),
			pImpl{ make_unique<Implementation>() }
		{}

		//Move constructor and assignment
		CustomerMapper(CustomerMapper&&) = default;
		CustomerMapper& operator=(CustomerMapper&&) = default;

		//Copy constructor and assignment
		CustomerMapper(const CustomerMapper&) = delete;
		CustomerMapper& operator=(const CustomerMapper&) = delete;

		~CustomerMapper() {}

		friend void swap(CustomerMapper& lhs, CustomerMapper& rhs)
		{
			using std::swap;

			swap(lhs.pImpl, rhs.pImpl);
		}

		virtual bool Update(Customer* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
		virtual bool Insert(Customer* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
		virtual bool Delete(Customer* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation);
	};
}