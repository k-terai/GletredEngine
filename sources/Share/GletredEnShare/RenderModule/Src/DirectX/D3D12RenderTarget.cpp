// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12RenderTarget.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"

using namespace std;
using namespace GletredEngine;

D3D12RenderTarget::D3D12RenderTarget() : RtvHeap(nullptr), RtvDescriptorSize(0)
{

}

D3D12RenderTarget::~D3D12RenderTarget()
{
	RtvHeap = nullptr;
	RtvDescriptorSize = 0;
}

void D3D12RenderTarget::Initialize(const ComPtr<CID3D12Device> device, const ComPtr<CIDXGISwapChain> swapChain, const uint32 frameCount)
{
	Targets.resize(frameCount);

	{
		D3D12_DESCRIPTOR_HEAP_DESC rtvHeapDesc = {};
		rtvHeapDesc.NumDescriptors = frameCount;
		rtvHeapDesc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV;
		rtvHeapDesc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE;
		const auto result = device->CreateDescriptorHeap(&rtvHeapDesc, IID_PPV_ARGS(&RtvHeap));
		SetExceptionIfFailed(result);
		RtvDescriptorSize = device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
	}

	{
		CD3DX12_CPU_DESCRIPTOR_HANDLE rtvHandle(RtvHeap->GetCPUDescriptorHandleForHeapStart());

		for (uint32 n = 0; n < frameCount; n++)
		{
			const auto result = swapChain->GetBuffer(n, IID_PPV_ARGS(&Targets[n]));
			SetExceptionIfFailed(result);
			device->CreateRenderTargetView(Targets[n].Get(), nullptr, rtvHandle);
			rtvHandle.Offset(1, RtvDescriptorSize);
		}
	}
}

void D3D12RenderTarget::Terminate()
{
	RtvHeap.Reset();
	RtvDescriptorSize = 0;

	for (auto t : Targets)
	{
		t.Reset();
	}
	Targets.clear();
}

void D3D12RenderTarget::ClearRenderTargetView(const ComPtr<ID3D12GraphicsCommandList> commandList, const uint32 index)
{
	CD3DX12_CPU_DESCRIPTOR_HANDLE rtvHandle(RtvHeap->GetCPUDescriptorHandleForHeapStart(), index, RtvDescriptorSize);
	const float clearColor[] = { 0.0f, 0.2f, 0.4f, 1.0f };
	commandList->ClearRenderTargetView(rtvHandle, clearColor, 0, nullptr);
}

