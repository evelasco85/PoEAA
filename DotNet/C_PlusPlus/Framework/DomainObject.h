#pragma once

#include <string>
#include "DllMacros.h"

using namespace std;

class Guid;
class BaseMapper;
class BaseQueryObject;

namespace Framework
{
	namespace Domain
	{
		class FRAMEWORK_API DomainObject
		{
		public:
			enum InstantiationType { New = 1, Loaded = 2 };
		private:
			BaseMapper *m_mapper;
			Guid *m_systemId;
			BaseQueryObject *m_queryObject;
		public:
			string GetTestMessage();
		};
	}
}