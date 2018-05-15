#include "stdafx.h"
#include "CppUnitTest.h"

#include "Customer.h"

using namespace std;
using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace CustomerServices;

namespace FrameworkTests
{		
	TEST_CLASS(CustomerTests)
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
			string customer1Guid = customer1.GetGuid();
			string customer1Number("100");
			string customer1Name("John Doe");
			
			customer1.SetNumber(customer1Number);
			customer1.SetName(customer1Name);

			Customer customer2(NULL);
			string customer2Guid = customer2.GetGuid();
			string customer2Number("200");
			string customer2Name("Jane Doe");

			customer2.SetNumber(customer2Number);
			customer2.SetName(customer2Name);

			//Identity test
			Assert::AreNotEqual(customer1Guid, customer2Guid, L"Should not be equal", LINE_INFO());
			
			//Test data setup
			Assert::AreEqual(customer1Number, customer1.GetNumber(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer1Name, customer1.GetName(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer2Number, customer2.GetNumber(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer2Name, customer2.GetName(), L"Should be equal", LINE_INFO());

			swap(customer1, customer2);		//Perform swap

			/*Test switching of Guid*/			
			Assert::AreEqual(customer1Guid, customer2.GetGuid(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer2Guid, customer1.GetGuid(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer1Number, customer2.GetNumber(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer1Name, customer2.GetName(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer2Number, customer1.GetNumber(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customer2Name, customer1.GetName(), L"Should be equal", LINE_INFO());
			/************************/
		}
	};
}