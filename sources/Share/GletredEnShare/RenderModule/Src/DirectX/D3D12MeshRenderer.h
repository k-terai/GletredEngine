// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "CoreModule/Inc/Resource.h"
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "RenderModule/Src/DirectX/D3D12GraphicsCommand.h"
#include "RenderModule/Src/DirectX/D3D12Material.h"
#include "RenderModule/Src/DirectX/D3D12MeshBase.h"

namespace GletredEngine
{
	class D3D12MeshRenderer : public Resource
	{
	public:
		D3D12MeshRenderer();
		~D3D12MeshRenderer() override;

		void Initialize(uniqueid meshId, uniqueid materialId);

		CID3D12RootSignature* GetRootSignature() const
		{
			return RootSignature.Get();
		}

		CID3D12PipelineState* GetPipelineState() const
		{
			return PipelineState.Get();
		}

		D3D12Material* GetMaterial() const
		{
			return  reinterpret_cast<D3D12Material*>(Material.get());
		}

		D3D12MeshBase* GetMesh() const
		{
			return  reinterpret_cast<D3D12MeshBase*>(Mesh.get());
		}

		void Render(const D3D12GraphicsCommand* command);

	private:
		void CreateEmptyRootSignature();
		void CreateSamplerRootSignature();
		void CreateGraphicsPipelineState();

		ComPtr<CID3D12Device> Device;
		std::shared_ptr<Resource> Mesh;
		std::shared_ptr<Resource> Material;
		ComPtr<CID3D12RootSignature> RootSignature;
		ComPtr<CID3D12PipelineState> PipelineState;
	};
}