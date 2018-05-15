#include "stdafx.h"

#include "BaseMapper.h"

#include "Customer.h"

using namespace CustomerServices;
using namespace Framework::DataManipulation;

class Customer::Implementation
{
private:
	string m_Number;
	string m_Name;
public:
	Implementation() :
		m_Number(),
		m_Name()
	{}
	Implementation(Implementation&&) = default;
	Implementation& operator=(Implementation&&) = default;
	Implementation(const Implementation&) = default;
	Implementation& operator=(const Implementation&) = default;

	~Implementation()
	{
		//Only objects instantiated within this class are to be destroyed
	}

	const string GetNumber() const { return m_Number; }
	void SetNumber(const string& number) { m_Number = number; }

	const string GetName() const { return m_Name; }
	void SetName(const string& name) { m_Name = name; }
};

Customer::Customer(ConstMapper* mapper) :
	DomainObject(mapper),
	pImpl{ make_unique<Implementation>() }
{
}

Customer::Customer(Customer&& rvalue) :
	DomainObject(move(rvalue)),
	pImpl(move(rvalue.pImpl))
{
}

Customer& Customer::operator=(Customer&& rvalue)
{
	DomainObject::operator=(move(rvalue));
	pImpl = move(rvalue.pImpl);

	return *this;
}

Customer::~Customer() = default;

const string Customer::GetNumber() const {
	if (!pImpl) return string("");

	return pImpl->GetNumber(); 
}

void Customer::SetNumber(const string& number) {
	if (pImpl) pImpl->SetNumber(number);
}

const string Customer::GetName() const {
	if (!pImpl) return string("");

	return pImpl->GetName(); 
}
void Customer::SetName(const string& name) {
	if (pImpl) pImpl->SetName(name);
}