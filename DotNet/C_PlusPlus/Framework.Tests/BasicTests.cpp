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
			DomainObject *customerDomainObject = new Customer(NULL);
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
			Customer sourceCustomer(NULL);
			string sourceGuidBeforeMove = sourceCustomer.GetGuid();
			Customer movedToCustomer = move(sourceCustomer);
			string guid = movedToCustomer.GetGuid();
			string sourceGuidAfterMove = sourceCustomer.GetGuid();

			Assert::AreEqual(sourceGuidBeforeMove, guid, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string(""), sourceGuidAfterMove, L"Should not contain guid", LINE_INFO());
		}

		TEST_METHOD(MoveAssignmentTest)
		{
			Customer sourceCustomer(NULL);
			Customer movedToCustomer(NULL);
			string sourceGuidBeforeMove = sourceCustomer.GetGuid();
			
			movedToCustomer = move(sourceCustomer);

			string guid = movedToCustomer.GetGuid();
			string sourceGuidAfterMove = sourceCustomer.GetGuid();

			Assert::AreEqual(sourceGuidBeforeMove, guid, L"Should be equal", LINE_INFO());
			Assert::AreEqual(string(""), sourceGuidAfterMove, L"Should not contain guid", LINE_INFO());
		}

		TEST_METHOD(SwapTest)
		{
			Customer customer1(NULL);
			Customer customer2(NULL);
			string customer1Guid = customer1.GetGuid();
			string customer2Guid = customer2.GetGuid();

			Assert::AreNotEqual(customer1Guid, customer2Guid, L"Should not be equal", LINE_INFO());

			swap(customer1, customer2);

			string newCustomer1Guid = customer1.GetGuid();
			string newCustomer2Guid = customer2.GetGuid();

			Assert::AreEqual(customer1Guid, newCustomer2Guid, L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer2Guid, newCustomer1Guid, L"Should be equal", LINE_INFO());
		}
	};
}