#include "stdafx.h"
#include "CustomerMapper.h"

using namespace std;
using namespace CustomerServices;

class CustomerMapper::Implementation
{
public:
	Implementation() = default;
	Implementation(Implementation&&) = default;
	Implementation& operator=(Implementation&&) = default;
	Implementation(const Implementation&) = default;
	Implementation& operator=(const Implementation&) = default;

	~Implementation()
	{
		//Only objects instantiated within this class are to be destroyed
	}	
};