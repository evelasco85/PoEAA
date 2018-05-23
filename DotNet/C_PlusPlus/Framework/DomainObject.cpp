#include "stdafx.h"

#include "Types.h"
#include "DomainObject.h"

using namespace std;
using namespace Framework;
using namespace Framework::Domain;

class FRAMEWORK_API DomainObject::Implementation
{
public:
	enum InstantiationType { New = 1, Loaded = 2 };
private:
	ConstGuid *m_systemId;				//Data non-modifiable
	Mapper *m_mapper;					//Data non-modifiable
public:
	Implementation(Mapper* mapper) :
		m_systemId(GetGuidString(NewGuid())),
		m_mapper(mapper) { }

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

	ConstGuid& GetGuid() const
	{
		return *m_systemId;
	}

	Mapper* GetMapper() const
	{
		return m_mapper;
	}
};

DomainObject::DomainObject(Mapper* mapper) :
	pImpl{ make_unique<Implementation>(mapper) } { }

DomainObject::DomainObject(DomainObject&& rvalue) :
	pImpl(move(rvalue.pImpl)) { }

DomainObject& DomainObject::operator=(DomainObject&& rvalue)
{
	pImpl = move(rvalue.pImpl);

	return *this;
}

//'default' explicityly informs compiler to generate body/func automatically
DomainObject::~DomainObject() = default;

//Inside a const member function, all non-static data members of the class becomes const.
DomainObject::ConstGuid DomainObject::GetGuid() const
{
	if (!pImpl) return string("");

	return pImpl->GetGuid();
}

//Inside a const member function, all non-static data members of the class becomes const.
DomainObject::Mapper* DomainObject::GetMapper() const
{
	if (!pImpl) return NULL;

	return pImpl->GetMapper();
}