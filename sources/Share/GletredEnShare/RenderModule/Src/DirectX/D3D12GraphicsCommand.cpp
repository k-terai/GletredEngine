// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12GraphicsCommand.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"

using namespace std;
using namespace GletredEngine;


D3D12GraphicsCommand::D3D12GraphicsCommand() : Device(nullptr), SwapChain(nullptr), CommandAllocator(nullptr), FenceEvent(nullptr),
FenceValue(1), FrameIndex(0), GraphicsCommandList(nullptr), CommandQueue(nullptr)
{

}

D3D12GraphicsCommand::~D3D12GraphicsCommand()
{
	Device.Reset();
	SwapChain.Reset();
	CommandAllocator.Reset();
	GraphicsCommandList.Reset();
	CommandQueue.Reset();
}

void D3D12GraphicsCommand::CreateCommandQueue(CID3D12Device* const device)
{
	D3D12_COMMAND_QUEUE_DESC queueDesc;
	SecureZeroMemory(&queueDesc, sizeof(D3D12_COMMAND_QUEUE_DESC));
	queueDesc.Flags = D3D12_COMMAND_QUEUE_FLAG_NONE;
	queueDesc.Type = D3D12_COMMAND_LIST_TYPE_DIRECT;
	const HRESULT result = device->CreateCommandQueue(&queueDesc, IID_PPV_ARGS(&CommandQueue));
	SetExceptionIfFailed(result);
}

void D3D12GraphicsCommand::Initialize(CID3D12Device* const device, CIDXGISwapChain* swapChain)
{
	Device = device;
	SwapChain = swapChain;

	CreateCommandAllocator(Device.Get());
	CreateGraphicsCommandList(Device.Get());
	CreateFence(Device.Get());

	FrameIndex = SwapChain->GetCurrentBackBufferIndex();
}


void D3D12GraphicsCommand::CreateCommandAllocator(CID3D12Device* const device)
{
	const HRESULT result = device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, IID_PPV_ARGS(&CommandAllocator));
	Logger::IsFailureLog(result);
}


void D3D12GraphicsCommand::CreateGraphicsCommandList(CID3D12Device* const device)
{
	const HRESULT result = device->CreateCommandList(0, D3D12_COMMAND_LIST_TYPE_DIRECT, CommandAllocator.Get(), nullptr, IID_PPV_ARGS(&GraphicsCommandList));
	Logger::IsFailureLog(result);
	GraphicsCommandList->Close();
}

void D3D12GraphicsCommand::CreateFence(CID3D12Device* const device)
{
	const HRESULT result = device->CreateFence(0, D3D12_FENCE_FLAG_NONE, IID_PPV_ARGS(&Fence));

	if (Logger::IsFailureLog(result))
	{
		return;
	}

	FenceValue = 1;
	FenceEvent = CreateEvent(nullptr, FALSE, FALSE, nullptr);
	if (FenceEvent == nullptr)
	{

	}
}

void D3D12GraphicsCommand::WaitForPreviousFrame()
{
	const uint64 fence = FenceValue;
	CommandQueue->Signal(Fence.Get(), fence);
	FenceValue++;

	if (Fence->GetCompletedValue() < fence)
	{
		Fence->SetEventOnCompletion(fence, FenceEvent);
		WaitForSingleObject(FenceEvent, INFINITE);
	}

	FrameIndex = SwapChain->GetCurrentBackBufferIndex();
}