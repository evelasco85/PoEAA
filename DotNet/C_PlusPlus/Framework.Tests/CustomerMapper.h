#pragma once

#include "stdafx.h"
#include <memory>

#include "Customer.h"
#include "BaseMapperConcrete.h"


using namespace std;
using namespace Framework::DataManipulation;

namespace CustomerServices
{
	class CustomerMapper : public BaseMapperConcrete<Customer>
	{
	public:
		CustomerMapper() : BaseMapperConcrete<Customer>(){}

		//Move constructor and assignment
		CustomerMapper(CustomerMapper&&) = default;
		CustomerMapper& operator=(CustomerMapper&&) = default;

		//Copy constructor and assignment
		CustomerMapper(const CustomerMapper&) = delete;
		CustomerMapper& operator=(const CustomerMapper&) = delete;

		~CustomerMapper() {}

		bool ConcreteUpdate(Customer* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
		{
			return false;
		}
	};
}