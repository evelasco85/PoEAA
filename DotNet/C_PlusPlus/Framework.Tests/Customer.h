#pragma once

#include "DomainObject.h"

using namespace std;
using namespace Framework::Domain;

namespace CustomerServices
{
	class Customer : public DomainObject
	{
	private:
	public:
		Customer() : DomainObject(nullptr){}

		//Move constructor and assignment
		Customer(Customer&&);
		Customer& operator=(Customer&&);

		//Copy constructor and assignment
		Customer(const Customer&) = delete;
		Customer& operator=(const Customer&) = delete;

		~Customer();
	};
}