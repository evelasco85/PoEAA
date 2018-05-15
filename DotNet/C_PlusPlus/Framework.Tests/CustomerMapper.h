#pragma once

#include "stdafx.h"
#include <memory>

#include "BaseMapperConcrete.h"

using namespace std;
using namespace Framework::DataManipulation;

namespace CustomerServices
{
	class Customer;

	class CustomerMapper : public BaseMapperConcrete<Customer>
	{
	private:
		class Implementation;
		unique_ptr<Implementation> pImpl;
	public:
		CustomerMapper();

		//Move constructor and assignment
		CustomerMapper(CustomerMapper&&) = default;
		CustomerMapper& operator=(CustomerMapper&&) = default;

		//Copy constructor and assignment
		CustomerMapper(const CustomerMapper&) = delete;
		CustomerMapper& operator=(const CustomerMapper&) = delete;

		~CustomerMapper();

		friend void swap(CustomerMapper& lhs, CustomerMapper& rhs)
		{
			using std::swap;

			swap(lhs.pImpl, rhs.pImpl);
		}

		/*Override BaseMapperConcrete<...> abstract functions*/
		bool ConcreteUpdate(Customer* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation);
		/****************************************/
	};
}