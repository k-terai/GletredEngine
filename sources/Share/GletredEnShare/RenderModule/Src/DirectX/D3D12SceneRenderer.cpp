// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12SceneRenderer.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"

using namespace  std;
using namespace  GletredEngine;

D3D12SceneRenderer::D3D12SceneRenderer() : Device(nullptr), Factory(nullptr),
FrameCount(2), FrameIndex(0)
{

}

D3D12SceneRenderer::~D3D12SceneRenderer()
{
	Device = nullptr;
	Factory = nullptr;
	SwapChain = nullptr;
	FrameCount = 2;
	FrameIndex = 0;
}

void D3D12SceneRenderer::Initialize(const ComPtr<CID3D12Device> device, const ComPtr<CIDXGIFactory> factory,
	const HWND hwnd)
{
	Device = device;
	Factory = factory;
	Command.CreateCommandQueue(device.Get());
	CreateSwapChain(hwnd);
	Command.Initialize(device, SwapChain);
	SwapChainRenderTarget.Initialize(device, SwapChain, FrameCount);
}

void D3D12SceneRenderer::Terminate()
{
	SwapChain.Reset();
	SwapChainRenderTarget.Terminate();
	Device.Reset();
	Factory.Reset();
	FrameCount = 2;
	FrameIndex = 0;
}

void D3D12SceneRenderer::CreateSwapChain(const HWND hwnd)
{
	RECT rect;
	int32 width;
	int32 height;

	if (GetWindowRect(hwnd, &rect))
	{
		width = rect.right - rect.left;
		height = rect.bottom - rect.top;
	}
	else
	{
		SetExceptionIfFailed(E_FAIL);
		return;
	}

	DXGI_SWAP_CHAIN_DESC1 swapChainDesc = {};
	swapChainDesc.BufferCount = FrameCount;
	swapChainDesc.Width = width;
	swapChainDesc.Height = height;
	swapChainDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
	swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD;
	swapChainDesc.SampleDesc.Count = 1;

	ComPtr<IDXGISwapChain1> swapChain;
	auto result = Factory->CreateSwapChainForHwnd(
		Command.GetCommandQueue(),
		hwnd,
		&swapChainDesc,
		nullptr,
		nullptr,
		&swapChain);
	SetExceptionIfFailed(result);

	swapChain.As(&SwapChain);
	SetExceptionIfFailed(result);
}

