// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Inc/RenderModule.h"
#include "RenderModule/Src/DirectX/D3D12Manager.h"

using namespace  std;
using namespace  GletredEngine;

void RenderModule::Startup()
{
	
}

void RenderModule::Shutdown()
{

}


void RenderModule::Initialize(GlobalData* data)
{
	data->RenderModule = this;
	D3D12Manager::GetInstance()->Initialize();
}

void RenderModule::Update()
{

}

void RenderModule::Terminate()
{

}



