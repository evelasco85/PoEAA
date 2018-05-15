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
		TEST_METHOD(Test)
		{
			Customer* customer = new Customer();
			CustomerMapper* customerMapper = new CustomerMapper();
			BaseMapper* mapper = customerMapper;
			SuccessfulInvocationDelegate successfulDelegate = [](const DomainObject&, const BaseMapperHashtable&)
			{
				//
			};
			FailedInvocationDelegate failedDelegate = [](const DomainObject&, const BaseMapperHashtable&)
			{
				//
			};

			string entityName = customerMapper->GetEntityTypeName();
			bool updated = mapper->Update(customer, successfulDelegate, failedDelegate);
			bool concreteUpdated = customerMapper->ConcreteUpdate(customer, successfulDelegate, failedDelegate);
		}
	};
}