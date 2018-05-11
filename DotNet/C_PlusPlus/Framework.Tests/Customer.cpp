#include "stdafx.h"
#include "Customer.h"

using namespace std;
using namespace CustomerServices;

class Customer::Implementation
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

Customer::Customer() :
	DomainObject(NULL, NULL),
	pImpl{ make_unique<Implementation>() }
{
}

Customer::Customer(Customer&& rvalue) :
	DomainObject(move(rvalue))
	,pImpl(move(rvalue.pImpl))
{
}

Customer& Customer::operator=(Customer&& rvalue)
{
	pImpl = move(rvalue.pImpl);
	DomainObject::operator=(move(rvalue));

	return *this;
}

Customer::~Customer() = default;