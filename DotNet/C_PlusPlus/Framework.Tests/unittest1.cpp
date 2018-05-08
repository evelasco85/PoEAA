#include "stdafx.h"
#include "CppUnitTest.h"
#include "Customer.h"

using namespace std;
using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace CustomerServices;

namespace FrameworkTests
{		
	TEST_CLASS(UnitTest1)
	{
	public:
		
		TEST_METHOD(TestMethod1)
		{
			DomainObject *customerDomainObject = new Customer;
			string expected("Hello World");

			Assert::AreEqual(expected, customerDomainObject->GetTestMessage(), L"", LINE_INFO());

			if (customerDomainObject != NULL)
			{
				delete customerDomainObject;

				customerDomainObject = NULL;
			}
		}
	};
}