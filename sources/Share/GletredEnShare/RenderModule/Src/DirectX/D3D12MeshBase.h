// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "CoreModule/Inc/Resource.h"

namespace GletredEngine
{
	class D3D12MeshBase : public Resource
	{
	public:
		D3D12MeshBase()
		{

		}
		~D3D12MeshBase() override
		{

		}

		D3D12_INPUT_LAYOUT_DESC GetInputLayoutDesc() const
		{
			return Layout;
		}

		D3D12_VERTEX_BUFFER_VIEW* GetVertexBufferView()
		{
			return &VertexBufferView;
		}

		virtual uint32 GetVertexCount() = 0;

	protected:
		ComPtr<CID3D12Device> Device;
		ComPtr<CID3D12Resource> VertexBuffer;
		uint32 VertexBufferSize = 0;
		D3D12_INPUT_LAYOUT_DESC Layout = {};
		D3D12_VERTEX_BUFFER_VIEW VertexBufferView = {};
	};
}
