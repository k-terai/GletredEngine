// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "CoreModule/Inc/NonCopyable.h"
#include "RenderModule/Src/DirectX/D3D12RenderTarget.h"

namespace GletredEngine
{
	class D3D12SceneRenderer : public NonCopyable
	{
	public:
		D3D12SceneRenderer();
		virtual ~D3D12SceneRenderer() override;
		void Initialize(ComPtr<CID3D12Device> device,
			ComPtr<CIDXGIFactory> factory,
			ComPtr<CID3D12CommandAllocator> allocator,
			HWND hwnd);

		virtual void Render() = 0;
		virtual void Terminate();

	protected:

		ComPtr<CID3D12Device> Device;
		ComPtr<CIDXGIFactory> Factory;
		ComPtr<CID3D12CommandAllocator> CommandAllocator;
		ComPtr<CID3D12CommandQueue> CommandQueue;
		ComPtr<CIDXGISwapChain> SwapChain;
		ComPtr<CID3D12GraphicsCommandList> CommandList;

		D3D12RenderTarget SwapChainRenderTarget;

		uint32 FrameCount;
		uint32 FrameIndex;

		HANDLE FenceEvent;
		ComPtr<ID3D12Fence> Fence;
		uint64 FenceValue;

		void WaitForPreviousFrame();

	private:
		void CreateGraphicsCommandQueue();
		void CreateSwapChain(HWND hwnd);
		void CreateGraphicsCommandList();
		void CreateFence();
	};
}
