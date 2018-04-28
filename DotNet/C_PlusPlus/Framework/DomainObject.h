#pragma once

#include <string>
#include "DllMacros.h"
#include <memory>

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
			unique_ptr<Guid> *m_systemId;
			shared_ptr<BaseMapper> *m_mapper;
			shared_ptr<BaseQueryObject> *m_queryObject;
		public:
			string GetTestMessage();
		};
	}
}