#pragma once

#include "stdafx.h"
#include <functional>
#include <unordered_map>

using namespace std;

namespace Framework
{
	namespace Domain
	{
		class DomainObject;
	}

	namespace DataManipulation
	{
		/*Type definifion visibility on Framework:DataManipulation namespace*/
		typedef unordered_map<string, string> BaseMapperHashtable;
		typedef function<void(const Domain::DomainObject*, const BaseMapperHashtable*)> InvocationDelegate;
		typedef InvocationDelegate SuccessfulInvocationDelegate;
		typedef InvocationDelegate FailedInvocationDelegate;
		/********************************************************************/
	}
}