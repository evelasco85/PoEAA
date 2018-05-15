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
			bool updated = genericMapper->Update(
				customer,
				[](const DomainObject&, const BaseMapperHashtable&)
			{
				//Successful Invocation
			},
				[](const DomainObject&, const BaseMapperHashtable&)
			{
				//Failed Invocation
			});
		}
	};
}