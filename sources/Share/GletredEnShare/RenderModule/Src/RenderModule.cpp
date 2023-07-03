// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Inc/RenderModule.h"
#include "RenderModule/Src/DirectX/D3D12Manager.h"
#include "RenderModule/Src/DirectX/D3D12SceneManager.h"
#include "RenderModule/Src/DirectX//D3D12ResourceManager.h"

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

#if GLETRED_ENGINE_PLATFORM_WINDOWS

	D3D12Manager::GetInstance()->Initialize(data->RenderData.UseWarpDevice);
	auto device = D3D12Manager::GetInstance()->GetDevice();
	auto factory = D3D12Manager::GetInstance()->GetFactory();

	D3D12SceneManager::GetInstance()->Initialize(device, factory, data->RenderData.SupportFullScreen);
	D3D12ResourceManager::GetInstance()->Initialize();

#endif


}

void RenderModule::Update()
{
#if GLETRED_ENGINE_PLATFORM_WINDOWS 
	D3D12SceneManager::GetInstance()->Update();
#endif

}

void RenderModule::Terminate()
{

#if GLETRED_ENGINE_PLATFORM_WINDOWS
	D3D12ResourceManager::GetInstance()->Terminate();

	D3D12Manager::GetInstance()->Terminate();
	D3D12SceneManager::GetInstance()->Terminate();
#endif

}

void RenderModule::CreateScene(RenderModuleSceneInitData data)
{
#if GLETRED_ENGINE_PLATFORM_WINDOWS 
	D3D12SceneManager::GetInstance()->CreateSceneRenderer(data.Hwnd);
#endif
}




