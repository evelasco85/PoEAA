#pragma once

#define FORCE_API_EXPORT _declspec(dllexport)

#ifdef FRAMEWORK_EXPORTS
#define FRAMEWORK_API FORCE_API_EXPORT
#else
#define FRAMEWORK_API _declspec(dllimport)
#endif
