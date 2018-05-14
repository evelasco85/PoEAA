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

bool CustomerMapper::Update(Customer* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	return false;
}

//
bool CustomerMapper::Insert(Customer* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	return false;
}
//

bool CustomerMapper::Delete(Customer* entity, SuccessfulInvocationDelegate* successfulInvocation, FailedInvocationDelegate* failedInvocation)
{
	return false;
}