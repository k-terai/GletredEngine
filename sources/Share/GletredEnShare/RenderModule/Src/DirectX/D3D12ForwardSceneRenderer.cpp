// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12ForwardSceneRenderer.h"

using namespace std;
using namespace GletredEngine;

D3D12ForwardSceneRenderer::D3D12ForwardSceneRenderer()
{

}

D3D12ForwardSceneRenderer::~D3D12ForwardSceneRenderer()
{

}

void D3D12ForwardSceneRenderer::Render()
{
	TestRender();
}

void D3D12ForwardSceneRenderer::TestRender()
{
	CommandAllocator->Reset();
	CommandList->Reset(CommandAllocator.Get(), nullptr);

	const auto t = CD3DX12_RESOURCE_BARRIER::Transition(SwapChainRenderTarget.GetResource(FrameIndex), D3D12_RESOURCE_STATE_PRESENT, D3D12_RESOURCE_STATE_RENDER_TARGET);
	CommandList->ResourceBarrier(1, &t);

	SwapChainRenderTarget.ClearRenderTargetView(CommandList, FrameIndex);

	const auto t2 = CD3DX12_RESOURCE_BARRIER::Transition(SwapChainRenderTarget.GetResource(FrameIndex), D3D12_RESOURCE_STATE_RENDER_TARGET, D3D12_RESOURCE_STATE_PRESENT);
	CommandList->ResourceBarrier(1, &t2);

	CommandList->Close();

	ID3D12CommandList* ppCommandLists[] = { CommandList.Get() };
	CommandQueue->ExecuteCommandLists(_countof(ppCommandLists), ppCommandLists);

	SwapChain->Present(1, 0);
	WaitForPreviousFrame();
}

