#pragma once

/*Disable Compiler Warning (level 1) C4251*/
#pragma warning(push)
#pragma warning(disable:4251)

//your declarations that cause 4251
#pragma warning(pop)
/******************************************/

#ifdef FRAMEWORK_EXPORTS
#define FRAMEWORK_API _declspec(dllexport)
#else
#define FRAMEWORK_API _declspec(dllimport)
#endif
