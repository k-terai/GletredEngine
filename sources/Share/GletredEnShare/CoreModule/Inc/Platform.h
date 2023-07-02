// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#if _WIN32 | _WIN64 | WINAPI_FAMILY
#define GLETRED_ENGINE_PLATFORM_WINDOWS 1
#endif

#if _XBOX_ONE | _GAMING_XBOX
#define _RIZA_ENGINE_PLATFORM_XBOX 1
#endif

#if GLETRED_ENGINE_PLATFORM_WINDOWS 
#define WIN32_LEAN_AND_MEAN       
#include<Windows.h> //Windows only
#endif

#include<tchar.h>
#include<string>

namespace GletredEngine
{
#if GLETRED_ENGINE_PLATFORM_WINDOWS 
	typedef TCHAR tchar;
	typedef std::basic_string<TCHAR> tstring;   // string,
	typedef LPTSTR ptstring;        // Pointer string.
	typedef LPCTSTR ctstring; 	    // Const string.
	typedef signed char int8;
	typedef unsigned char uint8;
	typedef unsigned short int uint16;
	typedef signed short int int16;
	typedef unsigned int uint32;
	typedef signed int int32;
	typedef unsigned long long uint64;
	typedef signed long long int64;
	typedef uint32 uniqueid;
	using WindowHandle = HWND;
#endif

}