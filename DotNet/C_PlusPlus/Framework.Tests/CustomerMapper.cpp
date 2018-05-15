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

const string CustomerMapper::SUCCESS_DESCRIPTION("Description");

bool CustomerMapper::ConcreteInsert(Customer* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation)
{
	BaseMapperHashtable results;

	if (entity == NULL)
	{
		failedInvocation(*entity, results);

		return false;
	}

	string data("test");

	EfficientAddOrUpdate(results, SUCCESS_DESCRIPTION, data);
	EfficientAddOrUpdate(results, SUCCESS_DESCRIPTION, data);

	successfulInvocation(*entity, results);

	return false;
}

bool CustomerMapper::ConcreteUpdate(Customer* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation)
{
	return false;
}

bool CustomerMapper::ConcreteDelete(Customer* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation)
{
	return false;
}