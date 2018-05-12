#pragma once

#include "DllMacros.h"

using namespace std;

class DomainObject;

namespace Framework
{
	namespace DataManipulation
	{
		//'Struct' where there are no internal state expected,
		//otherwise use class
		struct FRAMEWORK_API InvocationDelegate : public std::binary_function<DomainObject,
		{

		};
	}
}