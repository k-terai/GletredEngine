// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "Framework.h"
#include "CoreModule/Inc/GlobalData.h"
#include "FrameworkModule/Inc/FrameworkModule.h"

using namespace  GletredEngine;
using namespace  std;

GlobalData globalData;
FrameworkModule framework;


GLETREDEDENGINE_API void Initialize(HWND windowHandle)
{
	framework.Initialize(&globalData);
}

GLETREDEDENGINE_API void Startup()
{

}

GLETREDEDENGINE_API void Shutdown()
{
	
}

GLETREDEDENGINE_API void Update()
{

}

GLETREDEDENGINE_API void Terminate()
{

}
