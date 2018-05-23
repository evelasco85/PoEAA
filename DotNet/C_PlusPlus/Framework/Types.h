#pragma once

#include <memory>
#include <string>

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

	static inline const string* GetGuidString(const Guid* guid)
	{
		if (guid == NULL) return new string("");

		RPC_CSTR szUuid = NULL;
		string guidString;

		if (::UuidToStringA(guid, &szUuid) == RPC_S_OK)
		{
			guidString = (char*)szUuid;
			::RpcStringFreeA(&szUuid);

			return new string(guidString);
		}

		return new string("");
	}
#else
	typedef uuid_t Guid;
#endif
}