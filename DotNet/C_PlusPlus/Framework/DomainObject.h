#pragma once

#include <string>
#include <memory>
#include "Types.h"
#include "DllMacros.h"

using namespace std;

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
			const unique_ptr<Guid> m_systemId;		//Data non-modifiable
			const BaseMapper * const m_mapper;				//Data non-modifiable, pointer non-repointable
			const BaseQueryObject * const m_queryObject;	//Data non-modifiable, pointer non-repointable

		public:
			DomainObject():
				m_systemId(GenerateGuid()),
				m_mapper(NULL),
				m_queryObject(NULL)
			{
			}

			string GetTestMessage();
		};
	}
}