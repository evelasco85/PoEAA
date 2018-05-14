#include "stdafx.h"
#include "CppUnitTest.h"
#include "CustomerMapper.h"
#include "BaseMapperFunctions.h"
#include "BaseMapper.h"
#include "DomainObject.h"

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
			SuccessfulInvocationDelegate successfulDelegate = [](const DomainObject&, const BaseMapperHashtable&)
			{
				//
			};
			FailedInvocationDelegate failedDelegate = [](const DomainObject&, const BaseMapperHashtable&)
			{
				//
			};

			customerMapper->Update(
				customer,
				successfulDelegate,
				failedDelegate
			);
		}
	};
}