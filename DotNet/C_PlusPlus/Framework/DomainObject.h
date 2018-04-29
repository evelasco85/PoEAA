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
			typedef const unique_ptr<Guid> ConstGuidPtr;
			typedef const BaseMapper ConstMapper;
			typedef const BaseQueryObject ConstQueryObject;
			
		private:
			ConstGuidPtr m_systemId;		//Data non-modifiable
			ConstMapper *m_mapper;				//Data non-modifiable, pointer non-repointable
			ConstQueryObject *m_queryObject;	//Data non-modifiable, pointer non-repointable

		protected:
			DomainObject(ConstMapper *mapper) :
				m_systemId(GenerateGuid()),
				m_mapper(mapper),
				m_queryObject(nullptr)
			{
			}

		public:
			virtual ~DomainObject() = 0;		//Declaring this DomainObject as abstract class
			string GetTestMessage();
		};
	}
}