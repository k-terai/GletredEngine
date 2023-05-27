// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "FrameworkModule/Inc/FrameworkModule.h"
#include "RenderModule/Inc/RenderModule.h"

using namespace GletredEngine;

RenderModule renderModule;

FrameworkModule::FrameworkModule() : RenderModule(&renderModule)
{
	
}

FrameworkModule::~FrameworkModule()
{
	RenderModule = nullptr;
}


void FrameworkModule::Initialize(GlobalData* data)
{
	renderModule.Initialize(data);
}

void FrameworkModule::Startup()
{
	
}

void FrameworkModule::Shutdown()
{
	
}

void FrameworkModule::Update()
{
	
}

void FrameworkModule::Terminate()
{
	
}




