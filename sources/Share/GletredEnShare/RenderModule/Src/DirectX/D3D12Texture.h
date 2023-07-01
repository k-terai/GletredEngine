// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "CoreModule/Inc/Resource.h"

namespace GletredEngine
{
	class D3D12GraphicsCommand;

	class D3D12Texture : public Resource
	{
	public:
		D3D12Texture();
		~D3D12Texture() override;

		void InitializeAsCheckerBoard(ComPtr<CID3D12Device> device,uint32 width,uint32 height, uint32 pixelSize);
		void Destroy();

		void UploadGpu(D3D12GraphicsCommand* command);
		bool IsUploadGpu()  const
		{
			return IsUploadedGpu;
		}
		CID3D12DescriptorHeap* GetDescriptorHeap() const
		{
			return DescriptorHeap.Get();
		}

	private:
		int Index;
		D3D12_RESOURCE_DESC Desc;
		D3D12_SUBRESOURCE_DATA Data;
		ComPtr<CID3D12DescriptorHeap> DescriptorHeap;
		ComPtr<CID3D12Resource> TexResource;
		ComPtr<CID3D12Resource> TexHeap;
		bool IsUploadedGpu;

		static std::vector<uint8> GenerateCheckerBoard(uint32 width, uint32 height, uint32 pixelSize);
	};
}
