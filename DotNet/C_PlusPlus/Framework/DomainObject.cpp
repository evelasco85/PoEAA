#include <string>

#include "stdafx.h"
#include "DomainObject.h"

using namespace std;
using namespace Framework::Domain;

string DomainObject::GetTestMessage()
{
	 return "Hello World"; 
}

DomainObject::~DomainObject()
{
}
