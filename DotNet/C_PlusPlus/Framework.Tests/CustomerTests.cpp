#include "stdafx.h"
#include "CppUnitTest.h"
#include <unordered_map>

#include "Customer.h"
#include "ContainerUtility.h"

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

		TEST_METHOD(ModifyReferencesByReferenceWrapperTest)
		{
			typedef unordered_map<string, reference_wrapper<Customer>> Dictionary;

			Dictionary m_InternalData;
			Customer* customer1 = new Customer(NULL);
			Customer* customer2 = new Customer(NULL);
			string customerNo1 = "001";
			string customerName1 = "John Doe";
			string customerNo2 = "002";
			string customerName2 = "Jane Doe";

			customer1->SetNumber(customerNo1);
			customer1->SetName(customerName1);
			customer2->SetNumber(customerNo2);
			customer2->SetName(customerName2);

			EfficientAddOrUpdateByReferenceWrapper(m_InternalData, customer1->GetNumber(), *customer1);
			EfficientAddOrUpdateByReferenceWrapper(m_InternalData, customer2->GetNumber(), *customer2);

			Customer* retrievedCustomer1 = &GetValue(m_InternalData, customerNo1)->get();
			Customer* retrievedCustomer2 = &GetValue(m_InternalData, customerNo2)->get();

			Assert::IsTrue(m_InternalData.size() == 2, L"Should be equal", LINE_INFO());

			Assert::IsTrue(retrievedCustomer1 != NULL, L"Should not be NULL", LINE_INFO());
			Assert::AreEqual(customerNo1, retrievedCustomer1->GetNumber(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customerName1, retrievedCustomer1->GetName(), L"Should be equal", LINE_INFO());

			Assert::IsTrue(retrievedCustomer2 != NULL, L"Should not be NULL", LINE_INFO());
			Assert::AreEqual(customerNo2, retrievedCustomer2->GetNumber(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(customerName2, retrievedCustomer2->GetName(), L"Should be equal", LINE_INFO());

			string newCustomer1Name = "Juan Dela Cruz";
			string newCustomer2Name = "Juana Dela Cruz";

			retrievedCustomer1->SetName(newCustomer1Name);
			retrievedCustomer2->SetName(newCustomer2Name);
			
			Assert::AreEqual(newCustomer1Name, customer1->GetName(), L"Should be equal", LINE_INFO());
			Assert::AreEqual(newCustomer2Name, customer2->GetName(), L"Should be equal", LINE_INFO());
		}
	};
}