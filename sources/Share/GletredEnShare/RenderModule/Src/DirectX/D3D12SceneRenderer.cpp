// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12SceneRenderer.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"

using namespace  std;
using namespace  GletredEngine;

D3D12SceneRenderer::D3D12SceneRenderer() : Device(nullptr), Factory(nullptr), CommandQueue(nullptr), SwapChain(nullptr), CommandList(nullptr),
FrameCount(2), FrameIndex(0), FenceEvent(nullptr), Fence(nullptr), FenceValue(-1)
{

}

D3D12SceneRenderer::~D3D12SceneRenderer()
{
	Device = nullptr;
	Factory = nullptr;
	CommandQueue = nullptr;
	CommandList = nullptr;
	Fence = nullptr;
	FenceEvent = nullptr;
	SwapChain = nullptr;
	FenceValue = -1;
	FrameCount = 2;
	FrameIndex = 0;
}

void D3D12SceneRenderer::Initialize(const ComPtr<CID3D12Device> device, const ComPtr<CIDXGIFactory> factory, const ComPtr<CID3D12CommandAllocator> allocator,
	const HWND hwnd)
{
	Device = device;
	Factory = factory;
	CommandAllocator = allocator;
	CreateGraphicsCommandQueue();
	CreateSwapChain(hwnd);
	SwapChainRenderTarget.Initialize(device, SwapChain, FrameCount);
	CreateGraphicsCommandList();
	CreateFence();
}

void D3D12SceneRenderer::Terminate()
{
	WaitForPreviousFrame();

	SwapChain.Reset();
	SwapChainRenderTarget.Terminate();
	Device.Reset();
	Factory.Reset();
	CommandQueue.Reset();
	CommandList.Reset();
	Fence.Reset();
	FenceValue = -1;
	FrameCount = 2;
	FrameIndex = 0;
}

void D3D12SceneRenderer::CreateGraphicsCommandQueue()
{
	D3D12_COMMAND_QUEUE_DESC queueDesc = {};
	queueDesc.Flags = D3D12_COMMAND_QUEUE_FLAG_NONE;
	queueDesc.Type = D3D12_COMMAND_LIST_TYPE_DIRECT;
	const auto result = Device->CreateCommandQueue(&queueDesc, IID_PPV_ARGS(&CommandQueue));
	SetExceptionIfFailed(result);
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
		CommandQueue.Get(),
		hwnd,
		&swapChainDesc,
		nullptr,
		nullptr,
		&swapChain);
	SetExceptionIfFailed(result);

	swapChain.As(&SwapChain);
	SetExceptionIfFailed(result);
}

void D3D12SceneRenderer::CreateGraphicsCommandList()
{
	const auto result = Device->CreateCommandList(0, D3D12_COMMAND_LIST_TYPE_DIRECT,
		CommandAllocator.Get(),
		nullptr,
		IID_PPV_ARGS(&CommandList));
	SetExceptionIfFailed(result);

	CommandList->Close();

}

void D3D12SceneRenderer::CreateFence()
{
	const auto result = Device->CreateFence(0, D3D12_FENCE_FLAG_NONE, IID_PPV_ARGS(&Fence));
	SetExceptionIfFailed(result);

	FenceValue = 1;
	FenceEvent = CreateEvent(nullptr, FALSE, FALSE, nullptr);
	if (FenceEvent == nullptr)
	{
		SetExceptionIfFailed(HRESULT_FROM_WIN32(GetLastError()));
	}
}

void D3D12SceneRenderer::WaitForPreviousFrame()
{
	const uint64_t fence = FenceValue;
	auto result = CommandQueue->Signal(Fence.Get(), fence);
	SetExceptionIfFailed(result);
	FenceValue++;

	if (Fence->GetCompletedValue() < fence)
	{
		result = Fence->SetEventOnCompletion(fence, FenceEvent);
		SetExceptionIfFailed(result);
		WaitForSingleObject(FenceEvent, INFINITE);
	}

	FrameIndex = SwapChain->GetCurrentBackBufferIndex();
}

