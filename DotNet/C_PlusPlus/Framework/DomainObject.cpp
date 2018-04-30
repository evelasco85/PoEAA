#include <string>
#include "stdafx.h"
#include "Types.h"
#include "DomainObject.h"

using namespace std;
using namespace Framework::Domain;

class BaseQueryObject;

class DomainObject::Implementation
{
public:
	enum InstantiationType { New = 1, Loaded = 2 };
	typedef const unique_ptr<Guid> ConstGuidPtr;
	typedef const BaseQueryObject ConstQueryObject;
private:
	ConstGuidPtr m_systemId;			//Data non-modifiable
	ConstMapper *m_mapper;				//Data non-modifiable, pointer non-repointable
	ConstQueryObject *m_queryObject;	//Data non-modifiable, pointer non-repointable
public:
	Implementation(ConstMapper *mapper) :
		m_systemId(GenerateGuid()),
		m_mapper(mapper),
		m_queryObject(nullptr)
	{
	}
	~Implementation() {}

	//param type DomainObject is for back-reference
	string GetTestMessage(const DomainObject& domainObject) const
	{
		return "Hello World";
	}
};

DomainObject::DomainObject(ConstMapper *mapper) :
	pImpl{ make_unique<Implementation>(mapper) } { }

DomainObject::~DomainObject() { }

string DomainObject::GetTestMessage() const
{
	return pImpl->GetTestMessage(*this);
}