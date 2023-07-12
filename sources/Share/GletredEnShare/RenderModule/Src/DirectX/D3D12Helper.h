// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "CoreModule/Inc/Logger.h"

namespace GletredEngine
{
	inline tstring GetErrorMessage(const HRESULT hr)
	{
		LPWSTR messageBuffer = nullptr;
		const DWORD bufferSize = FormatMessageW(
			FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
			nullptr,
			hr,
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
			reinterpret_cast<LPWSTR>(&messageBuffer),
			0,
			nullptr
		);

		tstring errorMessage;
		if (bufferSize != 0) {
			errorMessage = std::wstring(messageBuffer, bufferSize);
			LocalFree(messageBuffer);
		}

		return errorMessage;
	}

	inline bool SetExceptionIfFailed(const HRESULT hr)
	{
		if (SUCCEEDED(hr))
		{
			return true;
		}

		Logger::Log(LogType::Exception, GetErrorMessage(hr).c_str());
		return false;
	}

	inline ScreenSize GetScreenSize(const WindowHandle hWnd)
	{
		RECT windowRect;
		ScreenSize size = {};

		if (GetWindowRect(hWnd, &windowRect))
		{
			int windowWidth = windowRect.right - windowRect.left;
			int windowHeight = windowRect.bottom - windowRect.top;
			size.Width = GetSystemMetrics(SM_CXSCREEN);
			size.Height = GetSystemMetrics(SM_CYSCREEN);
		}

		return size;
	}

}
