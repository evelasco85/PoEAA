#include <string>
#include "stdafx.h"
#include "Types.h"
#include "DomainObject.h"

using namespace std;
using namespace Framework::Domain;

class FRAMEWORK_API DomainObject::Implementation
{
public:
	enum InstantiationType { New = 1, Loaded = 2 };
	typedef const Guid ConstGuid;
private:
	ConstGuid *m_systemId;					//Data non-modifiable
	ConstMapper *m_mapper;					//Data non-modifiable
	ConstQueryObject *m_queryObject;		//Data non-modifiable
public:
	Implementation(ConstMapper* mapper, ConstQueryObject* queryObject) :
		m_systemId(GenerateGuid()),
		m_mapper(mapper),
		m_queryObject(queryObject)	{ }

	//Move constructor and assignment
	Implementation(Implementation&&) = default;
	Implementation& operator=(Implementation&&) = default;

	//Copy constructor and assignment, for strong guarantee and swapping
	Implementation(const Implementation&) = default;
	Implementation& operator=(const Implementation&) = default;

	~Implementation()
	{
		//Only objects instantiated within this class are to be destroyed
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

DomainObject::DomainObject(ConstMapper* mapper, ConstQueryObject* queryObject) :
	pImpl{ make_unique<Implementation>(mapper, queryObject) } { }

DomainObject::DomainObject(DomainObject&& rvalue) :
	pImpl(move(rvalue.pImpl)) { }

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