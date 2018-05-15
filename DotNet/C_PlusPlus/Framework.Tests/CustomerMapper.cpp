#include "stdafx.h"

#include "Customer.h"
#include "CustomerMapper.h"

using namespace CustomerServices;

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

CustomerMapper::CustomerMapper() :
	BaseMapperConcrete<Customer>(),
	pImpl{ make_unique<Implementation>() } { }

CustomerMapper::~CustomerMapper() {}

bool CustomerMapper::ConcreteUpdate(Customer* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation)
{
	BaseMapperHashtable table;

	successfulInvocation(*entity, table);

	return false;
}