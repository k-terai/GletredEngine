// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "CoreModule/Inc/Logger.h"
#include "CoreModule/Inc/Common.h"

using namespace std;
using namespace GletredEngine;

LogCallback Logger::SCallback;

void Logger::Log(const LogType type, ctstring fmt, ...)
{
	va_list args;
	va_start(args, fmt);
	LogVa(type, fmt, args);
	va_end(args);
}

#if GLETRED_ENGINE_PLATFORM_WINDOWS

bool Logger::IsFailureLog(const HRESULT hr)
{
	if (SUCCEEDED(hr))
	{
		return false;
	}

	return true;
}

#endif

void Logger::LogVa(const LogType type, ctstring fmt, va_list args)
{
	tchar buffer[C_BUFFER_SIZE];

	_vsnwprintf_s(buffer, C_BUFFER_SIZE, _TRUNCATE, fmt, args);
	if (SCallback)
	{
		SCallback(type, buffer);
	}
	else
	{
#ifdef UNICODE
		wprintf(buffer);
#else
		printf(buffer);
#endif

	}
}

tstring Logger::HrToString(const HRESULT hr)
{
	tchar str[64] = {};

#ifdef UNICODE
	wprintf(str, "HRESULT of 0x%08X", static_cast<uint32>(hr));
#else
	sprintf_s(str, "HRESULT of 0x%08X", static_cast<uint32>(hr));
#endif

	return tstring(str);
}