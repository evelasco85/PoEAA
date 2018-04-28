#pragma once

#ifdef FRAMEWORK_EXPORTS
#define FRAMEWORK_API _declspec(dllexport)
#else
#define FRAMEWORK_API _declspec(dllimport)
#endif
