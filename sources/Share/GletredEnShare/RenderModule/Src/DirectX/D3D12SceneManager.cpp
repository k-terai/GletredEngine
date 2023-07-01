// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12SceneManager.h"
#include "RenderModule/Src/DirectX/D3D12ForwardSceneRenderer.h"

using namespace GletredEngine;
using namespace std;

D3D12SceneManager::D3D12SceneManager() : SupportFullScreen(false), Device(nullptr), Factory(nullptr)
{

}

D3D12SceneManager::~D3D12SceneManager()
{

}

void D3D12SceneManager::Initialize(const ComPtr<CID3D12Device> device, const ComPtr<CIDXGIFactory> factory, const bool supportFullScreen)
{
	Device = device;
	Factory = factory;
	SupportFullScreen = supportFullScreen;
}

void D3D12SceneManager::Terminate()
{
	Device.Reset();
	Factory.Reset();

	for (const auto& r : SceneRendererVector)
	{
		r->Terminate();
	}
	SceneRendererVector.clear();

}

void D3D12SceneManager::CreateSceneRenderer(const HWND hwnd)
{
	SceneRendererVector.emplace_back(std::make_shared<D3D12ForwardSceneRenderer>());
	SceneRendererVector.back()->Initialize(Device, Factory, hwnd);

	if (SupportFullScreen)
	{
		//TODO: Fullscreen?
		Factory->MakeWindowAssociation(hwnd, 0);
	}
	else
	{
		Factory->MakeWindowAssociation(hwnd, DXGI_MWA_NO_ALT_ENTER);
	}
}

void D3D12SceneManager::Update()
{
	for (const auto& scene : SceneRendererVector)
	{
		scene->Render();
	}
}

