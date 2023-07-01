// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "CoreModule/Inc/NonCopyable.h"
#include <vector>

namespace GletredEngine
{
	class D3D12RenderTarget : public NonCopyable
	{
	public:
		D3D12RenderTarget();
		virtual ~D3D12RenderTarget() override;

		CID3D12Resource* GetResource(const uint32 index) const
		{
			return Targets[index].Get();
		}

		CD3DX12_CPU_DESCRIPTOR_HANDLE GetHandle(const uint32 index) const
		{
			const CD3DX12_CPU_DESCRIPTOR_HANDLE handle(RtvHeap->GetCPUDescriptorHandleForHeapStart(), index, RtvDescriptorSize);
			return handle;
		}

		void Initialize(ComPtr<CID3D12Device> device, ComPtr<CIDXGISwapChain> swapChain, uint32 frameCount);
		void Terminate();
		void ClearRenderTargetView(ComPtr<ID3D12GraphicsCommandList> commandList, uint32 index);


	private:
		ComPtr<CID3D12DescriptorHeap> RtvHeap;
		std::vector<ComPtr<CID3D12Resource>> Targets;
		uint32 RtvDescriptorSize;
	};
}
