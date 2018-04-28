#pragma once

#include <memory>
#include "DllMacros.h"

using namespace std;

extern "C"
{
#ifdef WIN32
#include <windows.h>
#include <Rpc.h>
#else
#include <uuid/uuid.h>
#endif
}

namespace Framework
{
#ifdef WIN32
	typedef UUID Guid;
	
	static unique_ptr<Guid> GenerateGuid()
	{
		Guid *uuid = new Guid;

		ZeroMemory(uuid, sizeof(Guid));

		if (UuidCreate(uuid) != RPC_S_OK)
		{
			fprintf(stderr, "Unable to create UUID!\n");
			exit(1);
		}

		return unique_ptr<Guid>(uuid);
	}
#else
	typedef uuid_t Guid;
#endif

	
}