#pragma once

extern "C"
{
#ifdef WIN32
#include <Rpc.h>
#else
#include <uuid/uuid.h>
#endif
}

namespace Framework
{
#ifdef WIN32
	typedef UUID Guid;
#else
	typedef uuid_t Guid;
#endif
}