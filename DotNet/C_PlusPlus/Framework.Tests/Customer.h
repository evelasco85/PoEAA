#pragma once

#include <memory> 

#include "DomainObject.h"

using namespace std;
using namespace Framework::Domain;
using namespace Framework::DataManipulation;

namespace Framework
{
	namespace DataManipulation
	{
		/*Class forward declaration*/
		class BaseMapper;
		/***************************/
	}
}

namespace CustomerServices
{
	class Customer : public DomainObject
	{
	private:
		class Implementation;
		unique_ptr<Implementation> pImpl;
	public:
		Customer(Mapper*);

		//Move constructor and assignment
		Customer(Customer&&);
		Customer& operator=(Customer&&);

		//Copy constructor and assignment
		Customer(const Customer&) = delete;
		Customer& operator=(const Customer&) = delete;

		~Customer();

		friend void swap(Customer& lhs, Customer& rhs)
		{
			using std::swap;

			DomainObject* lhsDomainObject = &lhs;
			DomainObject* rhsDomainObject = &rhs;

			swap(*lhsDomainObject, *rhsDomainObject);
			swap(lhs.pImpl, rhs.pImpl);
		}

		const string GetNumber() const;
		void SetNumber(const string& number);

		const string GetName() const;
		void SetName(const string& name);
	};
}