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
			Customer* customer = new Customer();
			CustomerMapper* customerMapper = new CustomerMapper();
			string concreteEntityName = customerMapper->GetEntityTypeName();
			BaseMapperConcrete<Customer>* genericMapper = customerMapper;
			string genericEntityName = genericMapper->GetEntityTypeName();
			string expectedName("class CustomerServices::Customer");

			Assert::AreEqual(expectedName, concreteEntityName, L"Should be equal", LINE_INFO());
			Assert::AreEqual(expectedName, genericEntityName, L"Should be equal", LINE_INFO());
		}

		TEST_METHOD(GenericPersistencyTest)
		{
			Customer* customer = new Customer();
			BaseMapper* genericMapper = new CustomerMapper();
			SuccessfulInvocationDelegate successfulDelegate = [](const DomainObject&, const BaseMapperHashtable&)
			{
				//
			};
			FailedInvocationDelegate failedDelegate = [](const DomainObject&, const BaseMapperHashtable&)
			{
				//
			};

			bool updated = genericMapper->Update(customer, successfulDelegate, failedDelegate);
		}
	};
}