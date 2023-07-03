// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"
#include "CoreModule/Inc/Resource.h"


namespace GletredEngine
{
	template<typename T>
	class D3D12Mesh : public Resource
	{
	public:
		HRESULT Initialize(const ComPtr<CID3D12Device> device, std::vector<T> vertexData, const D3D12_INPUT_LAYOUT_DESC layout)
		{
			Device = device;
			VertexData = vertexData;
			Layout = layout;
			VertexBufferSize = sizeof(T) * vertexData.size();

			const auto heap = CD3DX12_HEAP_PROPERTIES(D3D12_HEAP_TYPE_UPLOAD);
			const auto desc = CD3DX12_RESOURCE_DESC::Buffer(VertexBufferSize);

			const HRESULT result = Device->CreateCommittedResource(
				&heap,
				D3D12_HEAP_FLAG_NONE,
				&desc,
				D3D12_RESOURCE_STATE_GENERIC_READ,
				nullptr,
				IID_PPV_ARGS(&VertexBuffer));

			SetExceptionIfFailed(result);
			IsInitialized = true;
			return result;
		}

		void Destroy()
		{
			Device.Reset();
			VertexBuffer.Reset();
			VertexData.clear();

			Device = nullptr;
			VertexBuffer = nullptr;
			IsInitialized = false;
		}

		void Map(const std::vector<T>& data)
		{
			// Copy the triangle data to the vertex buffer.
			T* pVertexDataBegin;
			const CD3DX12_RANGE readRange(0, 0);  // GPU only.

			const auto r = VertexBuffer->Map(0, &readRange, reinterpret_cast<void**>(&pVertexDataBegin));
			if (SetExceptionIfFailed(r))
			{
				return;
			}

			std::copy(data.begin(), data.end(), pVertexDataBegin);
			VertexBuffer->Unmap(0, nullptr);

			// Initialize the vertex buffer view.
			VertexBufferView.BufferLocation = VertexBuffer->GetGPUVirtualAddress();
			VertexBufferView.StrideInBytes = sizeof(T);
			VertexBufferView.SizeInBytes = VertexBufferSize;
		}

		D3D12_INPUT_LAYOUT_DESC GetInputLayoutDesc() const
		{
			return Layout;
		}

		uint32 GetVertexCount() const
		{
			return VertexBufferSize / sizeof(T);
		}

		D3D12_VERTEX_BUFFER_VIEW* GetVertexBufferView()
		{
			return &VertexBufferView;
		}

	protected:
		ComPtr<CID3D12Device> Device;
		ComPtr<CID3D12Resource> VertexBuffer;
		D3D12_INPUT_LAYOUT_DESC Layout = {};
		D3D12_VERTEX_BUFFER_VIEW VertexBufferView = {};
		uint32 VertexBufferSize = 0;
		std::vector<T> VertexData;
	};
}