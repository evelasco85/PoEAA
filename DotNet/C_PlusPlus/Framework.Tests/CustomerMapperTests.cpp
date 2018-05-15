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
			Customer* customer = new Customer(NULL);
			string customerNumber("001");
			string customerName("John Doe");
			string resultCustomerNumber;
			string resultOperation;
			SuccessfulInvocationDelegate successInvocation = [&](const DomainObject& domainObject, const BaseMapperHashtable& result)
			{
				auto custNoResult = result.find(CustomerMapper::CUST_NO);

				if (custNoResult != result.end()) resultCustomerNumber = custNoResult->second;

				auto operationResult = result.find(CustomerMapper::OPERATION);

				if (operationResult != result.end()) resultOperation = operationResult->second;
			};
			FailedInvocationDelegate failedInvocation = [=](const DomainObject& domainObject, const BaseMapperHashtable& result)
			{ /*Failed Invocation*/ };

			customer->SetNumber(customerNumber);
			customer->SetName(customerName);

			bool updated = genericMapper->Insert(
				customer,
				successInvocation,
				failedInvocation);

			Assert::AreEqual(true, updated, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string("Insert"), resultOperation, L"Should be equal", LINE_INFO());
			Assert::AreEqual(customerNumber, resultCustomerNumber, L"Should be equal", LINE_INFO());
		}
	};
}