#include "stdafx.h"
#include "CppUnitTest.h"

#include "DomainObject.h"
#include "BaseMapper.h"
#include "BaseMapperFunctions.h"

#include "Customer.h"
#include "CustomerMapper.h"

using namespace std;
using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace CustomerServices;
using namespace Framework::Domain;
using namespace Framework::DataManipulation;

namespace FrameworkTests
{
	TEST_CLASS(CustomerMapperTests)
	{
	public:
		TEST_METHOD(GetEntityTypeNameTest)
		{
			Customer* customer = new Customer(NULL);
			CustomerMapper* customerMapper = new CustomerMapper();
			BaseMapperConcrete<Customer>* genericMapper = customerMapper;
			string concreteEntityName = customerMapper->GetEntityTypeName();
			string genericEntityName = genericMapper->GetEntityTypeName();
			string expectedName("class CustomerServices::Customer");

			Assert::AreEqual(expectedName, concreteEntityName, L"Should be equal", LINE_INFO());
			Assert::AreEqual(expectedName, genericEntityName, L"Should be equal", LINE_INFO());
		}

		TEST_METHOD(InsertTest)
		{
			CustomerMapper* customerMapper = new CustomerMapper();
			BaseMapper* genericMapper = customerMapper;
			Customer* customer1 = new Customer(NULL);
			string customer1Number("001");
			string customer1Name("John Doe");
			string latestResultCustomerNumber;
			string latestResultOperation;
			string latestResultCollectionCount;
			SuccessfulInvocationDelegate successInvocation = [&](const DomainObject* domainObject, const BaseMapperHashtable* result)
			{
				if ((domainObject == NULL) || (result == NULL)) return;

				auto custNoResult = result->find(CustomerMapper::CUST_NO);

				if (custNoResult != result->end()) latestResultCustomerNumber = custNoResult->second;

				auto operationResult = result->find(CustomerMapper::OPERATION);

				if (operationResult != result->end()) latestResultOperation = operationResult->second;

				auto collectionCountResult = result->find(CustomerMapper::COLLECTION_COUNT);

				if (collectionCountResult != result->end()) latestResultCollectionCount = collectionCountResult->second;
			};
			FailedInvocationDelegate failedInvocation = [=](const DomainObject* domainObject, const BaseMapperHashtable* result)
			{ /*Failed Invocation*/ };

			customer1->SetNumber(customer1Number);
			customer1->SetName(customer1Name);

			bool updated = genericMapper->Insert(
				customer1,
				&successInvocation,
				&failedInvocation);

			Assert::AreEqual(true, updated, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string("1"), latestResultCollectionCount, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string("Insert"), latestResultOperation, L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer1Number, latestResultCustomerNumber, L"Should be equal", LINE_INFO());
		}
	};
}