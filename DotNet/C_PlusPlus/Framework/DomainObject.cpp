#include <string>
#include "stdafx.h"
#include "Types.h"
#include "DomainObject.h"

using namespace std;
using namespace Framework::Domain;

class BaseQueryObject;

class FRAMEWORK_API DomainObject::Implementation
{
public:
	enum InstantiationType { New = 1, Loaded = 2 };
	typedef const Guid ConstGuid;
	typedef const BaseQueryObject ConstQueryObject;
private:
	ConstGuid *m_systemId;			//Data non-modifiable
	ConstMapper *m_mapper;					//Data non-modifiable, pointer non-repointable
	ConstQueryObject *m_queryObject;		//Data non-modifiable, pointer non-repointable
public:
	Implementation(ConstMapper *mapper) :
		m_systemId(GenerateGuid()),
		m_mapper(mapper),
		m_queryObject(NULL)	{ }

	~Implementation()
	{
		if (m_systemId != NULL)
		{
			delete m_systemId;

			m_systemId = NULL;
		}
	}

	const string GetGuid(const DomainObject&)
	{
		string guid;

		if (m_systemId == NULL) return string("");

		RPC_CSTR szUuid = NULL;

		if (::UuidToStringA(m_systemId, &szUuid) == RPC_S_OK)
		{
			guid = (char*)szUuid;
			::RpcStringFreeA(&szUuid);
		}

		return string(guid);
	}
};

DomainObject::DomainObject(ConstMapper *mapper) :
	pImpl{ make_unique<Implementation>(mapper) } { }

DomainObject::DomainObject(DomainObject&& rvalue) :
	pImpl(move(rvalue.pImpl))
{
}

DomainObject& DomainObject::operator=(DomainObject&& rvalue)
{
	pImpl = move(rvalue.pImpl);

	return *this;
}

//'default' explicityly informs compiler to generate body/func automatically
DomainObject::~DomainObject() = default;

const string DomainObject::GetGuid()
{
	if (!pImpl) return string("");

	return pImpl->GetGuid(*this);
}