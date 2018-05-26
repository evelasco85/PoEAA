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

		TEST_METHOD(NullMapperTest)
		{
			Customer* customer1 = new Customer(NULL);
			
			Assert::IsTrue(customer1->GetMapper() == NULL, L"Should be NULL", LINE_INFO());
		}

		TEST_METHOD(MapperTest)
		{
			CustomerMapper* customerMapper = new CustomerMapper();
			Customer* customer1 = new Customer(customerMapper);
			BaseMapper* genericMapper = customer1->GetMapper();

			Assert::IsTrue(genericMapper != NULL, L"Should not be NULL", LINE_INFO());
			Assert::AreEqual(string("class CustomerServices::Customer"), genericMapper->GetEntityTypeName(), L"Should be equal", LINE_INFO());
		}

		TEST_METHOD(InsertTest)
		{
			Customer* customer1 = new Customer(new CustomerMapper());
			BaseMapper* genericMapper = customer1->GetMapper();
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

			bool inserted = genericMapper->Insert(
				customer1,
				&successInvocation,
				&failedInvocation);

			Assert::AreEqual(true, inserted, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string("1"), latestResultCollectionCount, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string("Insert"), latestResultOperation, L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer1Number, latestResultCustomerNumber, L"Should be equal", LINE_INFO());
		}

		TEST_METHOD(MultipleInsertionTest)
		{
			CustomerMapper* mapper = new CustomerMapper();
			Customer* customer1 = new Customer(mapper);
			Customer* customer2 = new Customer(mapper);
			SuccessfulInvocationDelegate successInvocation = [&](const DomainObject* domainObject, const BaseMapperHashtable* result)
			{ };
			FailedInvocationDelegate failedInvocation = [=](const DomainObject* domainObject, const BaseMapperHashtable* result)
			{ /*Failed Invocation*/ };
			string customerNo1 = "001";
			string customerName1 = "John Doe";
			string customerNo2 = "002";
			string customerName2 = "Jane Doe";

			customer1->SetNumber(customerNo1);
			customer1->SetName(customerName1);
			customer2->SetNumber(customerNo2);
			customer2->SetName(customerName2);

			mapper->ConcreteInsert(
				customer1,
				&successInvocation,
				&failedInvocation);
			mapper->ConcreteInsert(
				customer2,
				&successInvocation,
				&failedInvocation);

			Customer* retrievedCustomer1 = mapper->GetCustomer(customerNo1);
			Customer* retrievedCustomer2 = mapper->GetCustomer(customerNo2);
			Customer* nonExistentCustomer = mapper->GetCustomer("DOES NOT EXISTS");

			Assert::IsTrue(mapper->GetCollectionCount() == 2, L"Should be equal", LINE_INFO());

			Assert::IsTrue(retrievedCustomer1 != NULL, L"Should not be NULL", LINE_INFO());
			Assert::AreEqual(customerNo1, retrievedCustomer1->GetNumber(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customerName1, retrievedCustomer1->GetName(), L"Should be equal", LINE_INFO());

			Assert::IsTrue(retrievedCustomer2 != NULL, L"Should not be NULL", LINE_INFO());
			Assert::AreEqual(customerNo2, retrievedCustomer2->GetNumber(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customerName2, retrievedCustomer2->GetName(), L"Should be equal", LINE_INFO());

			Assert::IsTrue(nonExistentCustomer == NULL, L"Should be NULL", LINE_INFO());
		}

		TEST_METHOD(UpdateTest)
		{
			BaseMapper* genericMapper = new CustomerMapper();
			string latestResultOperation;
			SuccessfulInvocationDelegate successInvocation = [&](const DomainObject* domainObject, const BaseMapperHashtable* result)
			{
				if ((domainObject == NULL) || (result == NULL)) return;

				auto operationResult = result->find(CustomerMapper::OPERATION);

				if (operationResult != result->end()) latestResultOperation = operationResult->second;

			};
			FailedInvocationDelegate failedInvocation = [=](const DomainObject* domainObject, const BaseMapperHashtable* result)
			{ /*Failed Invocation*/ };

			bool updated = genericMapper->Update(
				new Customer(genericMapper),
				&successInvocation,
				&failedInvocation);

			Assert::AreEqual(true, updated, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string("Update"), latestResultOperation, L"Should be equal", LINE_INFO());
		}

		TEST_METHOD(DeleteTest)
		{
			BaseMapper* genericMapper = new CustomerMapper();
			string latestResultOperation;
			SuccessfulInvocationDelegate successInvocation = [&](const DomainObject* domainObject, const BaseMapperHashtable* result)
			{
				if ((domainObject == NULL) || (result == NULL)) return;

				auto operationResult = result->find(CustomerMapper::OPERATION);

				if (operationResult != result->end()) latestResultOperation = operationResult->second;

			};
			FailedInvocationDelegate failedInvocation = [=](const DomainObject* domainObject, const BaseMapperHashtable* result)
			{ /*Failed Invocation*/ };

			bool deleted = genericMapper->Delete(
				new Customer(genericMapper),
				&successInvocation,
				&failedInvocation);

			Assert::AreEqual(true, deleted, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string("Delete"), latestResultOperation, L"Should be equal", LINE_INFO());
		}


		TEST_METHOD(DeleteMapperTest)
		{
			BaseMapper* genericMapper = new CustomerMapper();
			Customer* customer = new Customer(genericMapper);

			customer->SetNumber("001");
			customer->SetName("Test");
			genericMapper->Insert(customer, NULL, NULL);

			delete genericMapper;

			genericMapper = NULL;
		}
	};
}