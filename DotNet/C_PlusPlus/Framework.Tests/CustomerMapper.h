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

		bool ConcreteUpdate(Customer* entity, SuccessfulInvocationDelegate& successfulInvocation, FailedInvocationDelegate& failedInvocation);
	};

	CustomerMapper::CustomerMapper() :
		BaseMapperConcrete<Customer>(),
		pImpl{ make_unique<Implementation>() } { }

	CustomerMapper::~CustomerMapper() {}

	class CustomerMapper::Implementation
	{
	public:
		Implementation() = default;
		Implementation(Implementation&&) = default;
		Implementation& operator=(Implementation&&) = default;
		Implementation(const Implementation&) = default;
		Implementation& operator=(const Implementation&) = default;

		~Implementation()
		{
			//Only objects instantiated within this class are to be destroyed
		}
	};

	bool CustomerMapper::ConcreteUpdate(Customer* entity, SuccessfulInvocationDelegate& successfulInvocation, FailedInvocationDelegate& failedInvocation)
	{
		BaseMapperHashtable table;

		successfulInvocation(*entity, table);

		return false;
	}
}