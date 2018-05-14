#pragma once

#include <memory>

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
	
	static inline const Guid* NewGuid()
	{
		Guid uuid;

		ZeroMemory(&uuid, sizeof(Guid));

		if (UuidCreate(&uuid) != RPC_S_OK)
		{
			fprintf(stderr, "Unable to create UUID!\n");
			exit(1);
		}

		return new Guid(uuid);
	}
#else
	typedef uuid_t Guid;
#endif
}