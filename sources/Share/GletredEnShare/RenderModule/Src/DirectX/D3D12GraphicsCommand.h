// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "CoreModule/Inc/NonCopyable.h"
#include "RenderModule/Src/DirectX/D3D12Camera.h"
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "RenderModule/Src/DirectX/D3D12RenderTarget.h"

namespace GletredEngine
{
	class D3D12GraphicsCommand final : public NonCopyable
	{
	public:
		D3D12GraphicsCommand();
		~D3D12GraphicsCommand() override;
		void CreateCommandQueue(CID3D12Device* const device); //Public because called before init swapchain.
		void Initialize(CID3D12Device* const device, CIDXGISwapChain* swapChain);

		CID3D12CommandQueue* GetCommandQueue() const
		{
			return CommandQueue.Get();
		}

		CID3D12Device* GetDevice() const
		{
			return Device.Get();
		}

		void Reset() const
		{
			CommandAllocator->Reset();
			GraphicsCommandList->Reset(CommandAllocator.Get(), nullptr);
		}

		void TransitionPresentToRenderTarget(const D3D12RenderTarget& renderTarget) const
		{
			const CD3DX12_RESOURCE_BARRIER rb = CD3DX12_RESOURCE_BARRIER::Transition(renderTarget.GetResource(FrameIndex), D3D12_RESOURCE_STATE_PRESENT, D3D12_RESOURCE_STATE_RENDER_TARGET);
			GraphicsCommandList->ResourceBarrier(1, &rb);
		}

		void TransitionRenderTargetToPresent(const D3D12RenderTarget& renderTarget) const
		{
			const CD3DX12_RESOURCE_BARRIER rb = CD3DX12_RESOURCE_BARRIER::Transition(renderTarget.GetResource(FrameIndex), D3D12_RESOURCE_STATE_RENDER_TARGET, D3D12_RESOURCE_STATE_PRESENT);
			GraphicsCommandList->ResourceBarrier(1, &rb);
		}

		void TransitionDestToShaderResource(CID3D12Resource* resource) const
		{
			const CD3DX12_RESOURCE_BARRIER rb = CD3DX12_RESOURCE_BARRIER::Transition(resource, D3D12_RESOURCE_STATE_COPY_DEST, D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE);
			GraphicsCommandList->ResourceBarrier(1, &rb);
		}

		void SetRenderTargets(const D3D12RenderTarget& renderTarget) const
		{
			const CD3DX12_CPU_DESCRIPTOR_HANDLE handle = renderTarget.GetHandle(FrameIndex);
			GraphicsCommandList->OMSetRenderTargets(1, &handle, FALSE, nullptr);
		}

		void ClearRenderTargetView(const D3D12RenderTarget& renderTarget) const
		{
			const float clearColor[] = { 0.0f, 0.2f, 0.4f, 1.0f };
			GraphicsCommandList->ClearRenderTargetView(renderTarget.GetHandle(FrameIndex), clearColor, 0, nullptr);
		}

		void SetPrimitiveTopology(const D3D12_PRIMITIVE_TOPOLOGY primitiveTopology) const
		{
			GraphicsCommandList->IASetPrimitiveTopology(primitiveTopology);

		}

		void SetVertexBuffers(const D3D12_VERTEX_BUFFER_VIEW* view) const
		{
			GraphicsCommandList->IASetVertexBuffers(0, 1, view);
		}

		void SetGraphicsRootSignature(CID3D12RootSignature* rootSignature) const
		{
			GraphicsCommandList->SetGraphicsRootSignature(rootSignature);
		}

		void SetDescriptorHeaps(CID3D12DescriptorHeap* heap) const
		{
			CID3D12DescriptorHeap* ppHeaps[] = { heap };
			GraphicsCommandList->SetDescriptorHeaps(_countof(ppHeaps), ppHeaps);
		}

		void SetGraphicsRootDescriptorTable(CID3D12DescriptorHeap* heap) const
		{
			GraphicsCommandList->SetGraphicsRootDescriptorTable(0, heap->GetGPUDescriptorHandleForHeapStart());
		}

		void SetPipelineState(CID3D12PipelineState* pipelineState) const
		{
			GraphicsCommandList->SetPipelineState(pipelineState);
		}

		void SetViewportsAndScissorRects(D3D12Camera* const camera) const
		{
			GraphicsCommandList->RSSetViewports(1, camera->GetViewport());
			GraphicsCommandList->RSSetScissorRects(1, camera->GetRect());
		}

		void UpdateSubResources(CID3D12Resource* pDestinationResource, CID3D12Resource* pIntermediate, const D3D12_SUBRESOURCE_DATA* pSrcData) const
		{
			UpdateSubresources(GraphicsCommandList.Get(), pDestinationResource, pIntermediate, 0, 0, 1, pSrcData);
		}

		void DrawInstanced(const uint32 vertexCount, const uint32 instanceCount) const
		{
			GraphicsCommandList->DrawInstanced(vertexCount, instanceCount, 0, 0);
		}

		void Close() const
		{
			GraphicsCommandList->Close();
		}

		void Execute() const
		{
			ID3D12CommandList* ppCommandLists[] = { GraphicsCommandList.Get() };
			CommandQueue->ExecuteCommandLists(_countof(ppCommandLists), ppCommandLists);
		}

		void WaitForPreviousFrame();

		void Present() const
		{
			SwapChain->Present(1, 0);
		}

		void PresentAndWait()
		{
			Present();
			WaitForPreviousFrame();
		}


	private:
		void CreateCommandAllocator(CID3D12Device* const device);
		void CreateGraphicsCommandList(CID3D12Device* const device);
		void CreateFence(CID3D12Device* const device);

		ComPtr<CID3D12Device> Device;
		ComPtr<CIDXGISwapChain> SwapChain;
		ComPtr<CID3D12CommandQueue> CommandQueue;
		ComPtr<CID3D12CommandAllocator> CommandAllocator;
		ComPtr<CID3D12GraphicsCommandList> GraphicsCommandList;
		ComPtr<CID3D12Fence> Fence;
		HANDLE FenceEvent;
		uint64 FenceValue;
		uint32 FrameIndex;
	};
}
