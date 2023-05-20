// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "Common.h"
#include "Platform.h"

namespace GletredEngine
{
	class Logger
	{
	public:
		static void SetCallback(const LogCallback callback) { SCallback = callback; }
		static void Log(LogType type, ctstring fmt, ...);

#if GLETRED_ENGINE_PLATFORM_WINDOWS
		static bool IsFailureLog(HRESULT hr);
#endif

	public:
		static void LogVa(LogType type, ctstring fmt, va_list args);

	private:

#if GLETRED_ENGINE_PLATFORM_WINDOWS
		static tstring HrToString(HRESULT hr);
#endif

		static const int C_BUFFER_SIZE = 2048;
		static LogCallback SCallback;
	};
}