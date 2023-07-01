// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12Texture.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"
#include "RenderModule/Src/DirectX/D3D12GraphicsCommand.h"

using namespace std;
using namespace GletredEngine;

D3D12Texture::D3D12Texture() : Index(-1), IsUploadedGpu(false)
{
	SecureZeroMemory(&Desc, sizeof(D3D12_RESOURCE_DESC));
	SecureZeroMemory(&Data, sizeof(D3D12_SUBRESOURCE_DATA));
}

D3D12Texture::~D3D12Texture()
{
	DescriptorHeap = nullptr;
	TexResource = nullptr;
	TexHeap = nullptr;
	Index = -1;
	IsUploadedGpu = false;
}

void D3D12Texture::UploadGpu(D3D12GraphicsCommand* command)
{
	D3D12_SHADER_RESOURCE_VIEW_DESC srvDesc = {};
	SecureZeroMemory(&srvDesc, sizeof(D3D12_SHADER_RESOURCE_VIEW_DESC));

	srvDesc.Shader4ComponentMapping = D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING;
	srvDesc.Format = Desc.Format;
	srvDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE2D;
	srvDesc.Texture2D.MipLevels = 1;
	command->GetDevice()->CreateShaderResourceView(TexResource.Get(), &srvDesc, DescriptorHeap->GetCPUDescriptorHandleForHeapStart());

	const uint64 uploadBufferSize = GetRequiredIntermediateSize(TexResource.Get(), 0, 1);
	const auto properties = CD3DX12_HEAP_PROPERTIES(D3D12_HEAP_TYPE_UPLOAD);
	const auto buffer = CD3DX12_RESOURCE_DESC::Buffer(uploadBufferSize);

	auto result = command->GetDevice()->CreateCommittedResource(
		&properties,
		D3D12_HEAP_FLAG_NONE,
		&buffer,
		D3D12_RESOURCE_STATE_GENERIC_READ,
		nullptr,
		IID_PPV_ARGS(&TexHeap));

	SetExceptionIfFailed(result);

	command->Reset();
	command->UpdateSubResources(TexResource.Get(), TexHeap.Get(), &Data);
	command->TransitionDestToShaderResource(TexResource.Get());
	command->Close();
	command->Execute();
	command->WaitForPreviousFrame();

	IsUploadedGpu = true;
}

void D3D12Texture::InitializeAsCheckerBoard(const ComPtr<CID3D12Device> device, const uint32 width, const uint32 height, const uint32 pixelSize)
{
	D3D12_DESCRIPTOR_HEAP_DESC srvHeapDesc = {};
	SecureZeroMemory(&srvHeapDesc, sizeof(D3D12_DESCRIPTOR_HEAP_DESC));
	srvHeapDesc.NumDescriptors = 1;
	srvHeapDesc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV;
	srvHeapDesc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE;
	device->CreateDescriptorHeap(&srvHeapDesc, IID_PPV_ARGS(&DescriptorHeap));

	Desc.MipLevels = 1;
	Desc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
	Desc.Width = width;
	Desc.Height = height;
	Desc.Flags = D3D12_RESOURCE_FLAG_NONE;
	Desc.DepthOrArraySize = 1;
	Desc.SampleDesc.Count = 1;
	Desc.SampleDesc.Quality = 0;
	Desc.Dimension = D3D12_RESOURCE_DIMENSION_TEXTURE2D;

	const auto heap = CD3DX12_HEAP_PROPERTIES(D3D12_HEAP_TYPE_DEFAULT);

	const auto result = device->CreateCommittedResource(
		&heap,
		D3D12_HEAP_FLAG_NONE,
		&Desc,
		D3D12_RESOURCE_STATE_COPY_DEST,
		nullptr,
		IID_PPV_ARGS(&TexResource));



	const static auto data = GenerateCheckerBoard(width, height, pixelSize);
	Data.pData = &data[0];
	Data.RowPitch = static_cast<LONG_PTR>(width) * pixelSize;
	Data.SlicePitch = Data.RowPitch * height;
	IsInitialized = true;
}

void D3D12Texture::Destroy()
{
	DescriptorHeap.Reset();
	TexResource.Reset();
	TexHeap.Reset();
	Index = -1;
	IsUploadedGpu = false;
}


std::vector<uint8> D3D12Texture::GenerateCheckerBoard(const uint32 width, const uint32 height, const uint32 pixelSize)
{
	const uint32 rowPitch = width * pixelSize;
	const uint32 cellPitch = rowPitch >> 3;
	const uint32 cellHeight = width >> 3;
	const uint32 textureSize = rowPitch * height;

	std::vector<uint8> data(textureSize);
	uint8* pData = &data[0];

	for (uint32 n = 0; n < textureSize; n += pixelSize)
	{
		const uint32 x = n % rowPitch;
		const uint32 y = n / rowPitch;

		const uint32 i = x / cellPitch;
		const uint32 j = y / cellHeight;

		if (i % 2 == j % 2)
		{
			pData[n] = 0x00;        // R
			pData[n + 1] = 0x00;    // G
			pData[n + 2] = 0x00;    // B
			pData[n + 3] = 0xff;    // A
		}
		else
		{
			pData[n] = 0xff;        // R
			pData[n + 1] = 0xff;    // G
			pData[n + 2] = 0xff;    // B
			pData[n + 3] = 0xff;    // A
		}
	}

	return data;
}




