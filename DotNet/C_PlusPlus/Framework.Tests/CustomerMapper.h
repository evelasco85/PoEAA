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
	public:
		static const string CUST_NO;
		static const string CUST_NAME;
		static const string OPERATION;
		static const string COLLECTION_COUNT;
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
		bool ConcreteInsert(Customer* entity, const SuccessfulInvocationDelegate* successfulInvocation, const FailedInvocationDelegate* failedInvocation);
		bool ConcreteUpdate(Customer* entity, const SuccessfulInvocationDelegate* successfulInvocation, const FailedInvocationDelegate* failedInvocation);
		bool ConcreteDelete(Customer* entity, const SuccessfulInvocationDelegate* successfulInvocation, const FailedInvocationDelegate* failedInvocation);
		/****************************************/

		size_t GetCollectionCount() const;
		Customer* GetCustomer(const string&) const;
	};
}