#pragma once

#include <string>
#include <memory>
#include "DllMacros.h"

using namespace std;

class BaseMapper;

namespace Framework
{
	namespace Domain
	{
		class FRAMEWORK_API DomainObject									//Public facing operations
		{
		public:
			static const DomainObject null;
			typedef const BaseMapper ConstMapper;
		private:
			class Implementation;											//State persistency
			unique_ptr<Implementation> pImpl;								//pImpl is a complete type
		protected:															//Declaring DomainObject uninstantiable
			DomainObject(ConstMapper*);

			//Move constructor and assignment
			DomainObject(DomainObject&&);
			DomainObject& operator=(DomainObject&&);

			//Copy constructor and assignment
			DomainObject(const DomainObject&) = delete;						//'delete' informs compiler not to generate body/func automatically
			DomainObject& operator=(const DomainObject&) = delete;			//'delete' informs compiler not to generate body/func automatically
		public:
			virtual ~DomainObject() = 0;									//Declaring DomainObject as abstract class

			string GetTestMessage() const;
		};
	}
}