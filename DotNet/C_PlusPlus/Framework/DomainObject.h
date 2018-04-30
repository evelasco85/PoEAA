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
		class FRAMEWORK_API DomainObject
		{
		public:
			static const DomainObject null;
			typedef const BaseMapper ConstMapper;
		private:
			class Implementation;
			unique_ptr<Implementation> pImpl;
		protected:								//Declaring DomainObject uninstantiable
			DomainObject(ConstMapper*);
		public:
			virtual ~DomainObject() = 0;		//Declaring DomainObject as abstract class
			string GetTestMessage() const;
		};
	}
}