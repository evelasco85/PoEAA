#include "stdafx.h"

#include <algorithm>

#include "Customer.h"
#include "CustomerMapper.h"
#include "ContainerUtility.h"

using namespace CustomerServices;

class CustomerMapper::Implementation
{
private:
	/*
	Reference Wrapper: Elements referred to in a container
	 exists as long as the container exists
	*/
	typedef unordered_map<string, Customer*> Dictionary;
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

		/*Remove container elements, but does not delete elements(pointers)*/
		//m_InternalData.clear();
		/*******************************************************************/

		/*Alternatively, variation of Item-33*/
		for_each(m_InternalData.begin(), m_InternalData.end(),
			[](pair<const string, Customer*> &element) {

			/*Delete object*/
			delete element.second;
			element.second = NULL;
			/******************/
		});

		MapRemoveValues(m_InternalData, [](pair<const string, Customer*> element) {
			return (element.second == NULL);
		});
		/*************************************/
	}

	void AddEditCustomer(Customer& customer)
	{
		EfficientAddOrUpdateByPtr(m_InternalData, customer.GetNumber(), &customer);
	}

	size_t CollectionCount() const
	{
		return m_InternalData.size();
	}

	Customer* GetCustomer(const string& customerNumber) const
	{
		Customer **retVal = GetValue(m_InternalData, customerNumber);

		return (retVal == NULL) ? NULL : (Customer*)(*retVal);
	}
};

CustomerMapper::CustomerMapper() :
	BaseMapperConcrete<Customer>(),
	pImpl{ make_unique<Implementation>() } { }

CustomerMapper::~CustomerMapper() {}

const string CustomerMapper::CUST_NO("Customer Number");
const string CustomerMapper::CUST_NAME("Customer Name");
const string CustomerMapper::OPERATION("Operation");
const string CustomerMapper::COLLECTION_COUNT("Collection Count");

bool CustomerMapper::ConcreteInsert(Customer* entity, const SuccessfulInvocationDelegate* successfulInvocation, const FailedInvocationDelegate* failedInvocation)
{
	BaseMapperHashtable results;

	if (entity == NULL)
	{
		if(failedInvocation != NULL) (*failedInvocation)(entity, &results);

		return false;
	}

	if (pImpl) pImpl->AddEditCustomer(*entity);

	EfficientAddOrUpdate(results, CUST_NO, entity->GetNumber());
	EfficientAddOrUpdate(results, CUST_NAME, entity->GetName());
	EfficientAddOrUpdate(results, OPERATION, "Insert");
	EfficientAddOrUpdate(results, COLLECTION_COUNT, to_string(pImpl->CollectionCount()));

	if(successfulInvocation != NULL) (*successfulInvocation)(entity, &results);

	return true;
}

bool CustomerMapper::ConcreteUpdate(Customer* entity, const SuccessfulInvocationDelegate* successfulInvocation, const FailedInvocationDelegate* failedInvocation)
{
	BaseMapperHashtable results;

	if (entity == NULL)
	{
		if (failedInvocation != NULL) (*failedInvocation)(entity, &results);

		return false;
	}

	EfficientAddOrUpdate(results, OPERATION, "Update");

	if (successfulInvocation != NULL) (*successfulInvocation)(entity, &results);

	return true;
}

bool CustomerMapper::ConcreteDelete(Customer* entity, const SuccessfulInvocationDelegate* successfulInvocation, const FailedInvocationDelegate* failedInvocation)
{

	BaseMapperHashtable results;

	if (entity == NULL)
	{
		if (failedInvocation != NULL) (*failedInvocation)(entity, &results);

		return false;
	}

	EfficientAddOrUpdate(results, OPERATION, "Delete");

	if (successfulInvocation != NULL) (*successfulInvocation)(entity, &results);

	return true;
}

size_t CustomerMapper::GetCollectionCount() const
{
	return (!pImpl) ? 0 : pImpl->CollectionCount();
}

Customer* CustomerMapper::GetCustomer(const string& customerNumber) const
{
	return (!pImpl) ? NULL : pImpl->GetCustomer(customerNumber);
}