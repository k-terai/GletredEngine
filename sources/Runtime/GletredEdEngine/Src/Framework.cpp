// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "Framework.h"
#include "CoreModule/Inc/GlobalData.h"
#include "CoreModule/Inc/IRenderModule.h"
#include "FrameworkModule/Inc/FrameworkModule.h"

using namespace  GletredEngine;
using namespace  std;

GlobalData globalData;
FrameworkModule framework;


GLETREDEDENGINE_API void Initialize()
{
	globalData.RenderData.SupportFullScreen = false;
	globalData.RenderData.UseWarpDevice = false;
	framework.Initialize(&globalData);
	framework.Update();
}

GLETREDEDENGINE_API void Startup()
{
	framework.Startup();
}

GLETREDEDENGINE_API void Shutdown()
{
	framework.Shutdown();
}

GLETREDEDENGINE_API void Update()
{
	framework.Update();
}

GLETREDEDENGINE_API void Terminate()
{
	framework.Terminate();
}

GLETREDEDENGINE_API void CreateScene(const HWND hwnd)
{
	RenderModuleSceneInitData data{};
	data.Hwnd = hwnd;
	framework.GetRenderModule()->CreateScene(data);
}