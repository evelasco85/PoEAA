#include "stdafx.h"

#include "Customer.h"
#include "CustomerMapper.h"

using namespace CustomerServices;

class CustomerMapper::Implementation
{
private:
	typedef unordered_map<string, reference_wrapper<Customer>> Dictionary;
private:
	Dictionary m_InternalData;
public:
	Implementation() :
		m_InternalData()
	{ }
	Implementation(Implementation&&) = default;
	Implementation& operator=(Implementation&&) = default;
	Implementation(const Implementation&) = default;
	Implementation& operator=(const Implementation&) = default;

	~Implementation()
	{
		//Only objects instantiated within this class are to be destroyed
	}

	void AddEditCustomer(Customer& customer)
	{
		EfficientAddOrUpdateByRef(m_InternalData, customer.GetNumber(), customer);
	}
};

CustomerMapper::CustomerMapper() :
	BaseMapperConcrete<Customer>(),
	pImpl{ make_unique<Implementation>() } { }

CustomerMapper::~CustomerMapper() {}

const string CustomerMapper::CUST_NO("Customer Number");
const string CustomerMapper::CUST_NAME("Customer Name");
const string CustomerMapper::OPERATION("Operation");

bool CustomerMapper::ConcreteInsert(Customer* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation)
{
	BaseMapperHashtable results;

	if (entity == NULL)
	{
		failedInvocation(*entity, results);

		return false;
	}

	if (pImpl) pImpl->AddEditCustomer(*entity);

	EfficientAddOrUpdate(results, CUST_NO, entity->GetNumber());
	EfficientAddOrUpdate(results, CUST_NAME, entity->GetName());
	EfficientAddOrUpdate(results, OPERATION, "Insert");
	successfulInvocation(*entity, results);

	return true;
}

bool CustomerMapper::ConcreteUpdate(Customer* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation)
{
	return false;
}

bool CustomerMapper::ConcreteDelete(Customer* entity, const SuccessfulInvocationDelegate& successfulInvocation, const FailedInvocationDelegate& failedInvocation)
{
	return false;
}