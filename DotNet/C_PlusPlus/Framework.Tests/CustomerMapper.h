#pragma once

#include "stdafx.h"
#include "BaseMapper.h"
#include <memory>

using namespace std;
using namespace Framework::DataManipulation;

class Customer;

namespace CustomerServices
{
	class CustomerMapper : public BaseMapperSpecific<Customer>
	{
	private:
		class Implementation;
		unique_ptr<Implementation> pImpl;
	public:
		CustomerMapper();

		//Move constructor and assignment
		CustomerMapper(CustomerMapper&&);
		CustomerMapper& operator=(CustomerMapper&&);

		//Copy constructor and assignment
		CustomerMapper(const CustomerMapper&) = delete;
		CustomerMapper& operator=(const CustomerMapper&) = delete;

		~CustomerMapper();

		friend void swap(CustomerMapper& lhs, CustomerMapper& rhs)
		{
			using std::swap;

			swap(lhs.pImpl, rhs.pImpl);
		}
	};
}