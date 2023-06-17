// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "CoreModule/Inc/Platform.h"

namespace GletredEngine
{
	struct RenderModuleData
	{
		WindowHandle Hwnd;
		bool UseWarpDevice;
		bool SupportFullScreen;
	};
}
