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
			Customer customer;
			string expected("Hello World");

			Assert::AreEqual(expected, customer.GetTestMessage(), L"", LINE_INFO());
		}
	};
}