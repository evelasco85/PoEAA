#include "stdafx.h"
#include "CppUnitTest.h"
#include "Customer.h"

using namespace std;
using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace CustomerServices;

namespace FrameworkTests
{		
	TEST_CLASS(BasicTests)
	{
	public:
		
		TEST_METHOD(PolymorphismTest)
		{
			DomainObject *customerDomainObject = new Customer;
			const string guid = customerDomainObject->GetGuid();

			Assert::AreNotEqual(string(""), guid, L"Should not be null", LINE_INFO());

			if (customerDomainObject != NULL)
			{
				delete customerDomainObject;

				customerDomainObject = NULL;
			}
		}

		TEST_METHOD(MoveConstructorTest)
		{
			Customer sourceCustomer;
			string sourceGuidBeforeMove = sourceCustomer.GetGuid();
			Customer movedToCustomer = move(sourceCustomer);
			string guid = movedToCustomer.GetGuid();
			string sourceGuidAfterMove = sourceCustomer.GetGuid();

			Assert::AreEqual(sourceGuidBeforeMove, guid, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string(""), sourceGuidAfterMove, L"Should not contain guid", LINE_INFO());
		}

		TEST_METHOD(MoveAssignmentTest)
		{
			Customer sourceCustomer;
			Customer movedToCustomer;
			string sourceGuidBeforeMove = sourceCustomer.GetGuid();
			
			movedToCustomer = move(sourceCustomer);

			string guid = movedToCustomer.GetGuid();
			string sourceGuidAfterMove = sourceCustomer.GetGuid();

			Assert::AreEqual(sourceGuidBeforeMove, guid, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string(""), sourceGuidAfterMove, L"Should not contain guid", LINE_INFO());
		}
	};
}