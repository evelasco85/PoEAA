#pragma once

#include <string>
#include <memory>
#include "DllMacros.h"

using namespace std;

namespace Framework
{
	namespace DataManipulation
	{
		//Forward declaration
		class BaseMapper;
	}

	namespace Domain
	{
		class BaseQueryObject;

		class FRAMEWORK_API DomainObject									//Public facing operations
		{
		public:
			typedef const DataManipulation::BaseMapper ConstMapper;
			typedef const BaseQueryObject ConstQueryObject;
		private:
			class Implementation;											//State persistency
			unique_ptr<Implementation> pImpl;								//pImpl is a complete type
		protected:															//Declaring DomainObject uninstantiable
			DomainObject(ConstMapper*, ConstQueryObject*);

			//Move constructor and assignment
			DomainObject(DomainObject&&);
			DomainObject& operator=(DomainObject&&);

			//Copy constructor and assignment
			DomainObject(const DomainObject&) = delete;						//'delete' informs compiler not to generate body/func automatically, thus prevents copying
			DomainObject& operator=(const DomainObject&) = delete;			//'delete' informs compiler not to generate body/func automatically, thus prevents copying
		public:
			virtual ~DomainObject() = 0;									//Declaring DomainObject as abstract class

			friend void swap(DomainObject& lhs, DomainObject& rhs)
			{
				using std::swap;

				swap(lhs.pImpl, rhs.pImpl);
			}

			const string GetGuid() const;
		};
	}
}