#pragma once

#include <string>
#include "BaseMapper.h"
#include "Types.h"
#include "BaseQueryObject.h"

using namespace std;
using namespace Framework;
using namespace Framework::DataManipulation;

#ifdef FRAMEWORK_EXPORTS
#define DOMAINOBJECT_API _declspec(dllexport)
#else
#define DOMAINOBJECT_API _declspec(dllimport)
#endif

namespace Framework
{
	namespace Domain
	{
		class DOMAINOBJECT_API DomainObject
		{
		public:
			enum InstantiationType { New = 1, Loaded = 2 };
		private:
			BaseMapper m_mapper;
			Guid m_systemId;
			BaseQueryObject m_queryObject;
		public:
			string GetTestMessage();
		};
	}
}